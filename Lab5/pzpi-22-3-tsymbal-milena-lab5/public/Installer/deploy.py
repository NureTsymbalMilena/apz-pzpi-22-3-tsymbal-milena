import zipfile
import os
import sys
import subprocess
import threading
import queue
import time
from datetime import datetime
import shutil

def log(message):
    try:
        with open("deploy.log", "a", encoding="utf-8") as log_file:
            log_file.write(f"[{datetime.now()}] {message}\n")
        print(message)
    except Exception as e:
        print(f"[WARNING] Error writing to log: {e}")

def check_requirements():
    """Check for Node.js and .NET SDK presence"""
    try:
        result = subprocess.run(["node", "--version"], check=True, capture_output=True, text=True)
        log(f"[OK] Node.js is installed: {result.stdout.strip()}")
    except subprocess.CalledProcessError as e:
        log(f"[ERROR] Error checking Node.js: {e.stderr.strip()}. Install from https://nodejs.org/")
        return False
    except FileNotFoundError:
        log("[ERROR] Node.js not found in PATH. Install from https://nodejs.org/")
        return False
    try:
        result = subprocess.run(["dotnet", "--version"], check=True, capture_output=True, text=True)
        log(f"[OK] .NET SDK is installed: {result.stdout.strip()}")
    except subprocess.CalledProcessError as e:
        log(f"[ERROR] Error checking .NET SDK: {e.stderr.strip()}. Install from https://dotnet.microsoft.com/download/dotnet/8.0")
        return False
    except FileNotFoundError:
        log("[ERROR] .NET SDK not found in PATH. Install from https://dotnet.microsoft.com/download/dotnet/8.0")
        return False
    return True

def run_process(command, cwd, name, output_queue):
    """Run process with output redirection to queue"""
    try:
        process = subprocess.Popen(
            command,
            cwd=cwd,
            shell=True,
            stdout=subprocess.PIPE,
            stderr=subprocess.STDOUT,
            text=True,
            encoding='utf-8',
            errors='replace'
        )
        while True:
            output = process.stdout.readline()
            if output == '' and process.poll() is not None:
                break
            if output:
                output_queue.put(f"[{name}] {output.strip()}")
        return_code = process.poll()
        if return_code != 0:
            output_queue.put(f"[ERROR] {name} exited with code {return_code}")
    except Exception as e:
        output_queue.put(f"[ERROR] Error running {name}: {str(e)}")

def main():
    if len(sys.argv) < 3:
        log("[ERROR] Insufficient arguments. Usage: deploy.py ZIP_PATH EXTRACT_TO")
        return

    zip_path = sys.argv[1]
    extract_to = sys.argv[2]

    log(f"[INFO] ZIP_PATH: {zip_path}")
    log(f"[INFO] EXTRACT_TO: {extract_to}")

    if not os.path.exists(zip_path):
        log(f"[ERROR] ZIP file not found: {zip_path}")
        return

    if not check_requirements():
        log("[ERROR] Required dependencies are missing. Deployment aborted.")
        return

    try:
        os.makedirs(extract_to, exist_ok=True)
        log(f"[INFO] Extracting: {zip_path} -> {extract_to}")
        try:
            with zipfile.ZipFile(zip_path, 'r') as zip_ref:
                zip_ref.extractall(extract_to)
        except zipfile.BadZipFile:
            log(f"[ERROR] ZIP file is corrupted or not an archive: {zip_path}")
            return

        backend_path = os.path.join(extract_to, 'InRoom', 'server', 'InRoom', 'InRoom.API')
        frontend_path = os.path.join(extract_to, 'InRoom', 'web', 'InRoom')

        log(f"[INFO] Backend path: {backend_path}")
        log(f"[INFO] Frontend path: {frontend_path}")

        if not os.path.exists(backend_path):
            log(f"[ERROR] Backend path not found: {backend_path}")
            return

        if not os.path.exists(frontend_path):
            log(f"[ERROR] Frontend path not found: {frontend_path}")
            return

        package_json = os.path.join(frontend_path, 'package.json')
        if not os.path.exists(package_json):
            log(f"[ERROR] package.json file not found: {package_json}")
            return

        csproj_file = os.path.join(backend_path, 'InRoom.API.csproj')
        if not os.path.exists(csproj_file):
            log(f"[ERROR] InRoom.API.csproj file not found: {csproj_file}")
            return

        log("[INFO] Cleaning bin and obj folders...")
        try:
            bin_path = os.path.join(backend_path, 'bin')
            obj_path = os.path.join(backend_path, 'obj')
            if os.path.exists(bin_path):
                shutil.rmtree(bin_path, ignore_errors=True)
                log("[OK] bin folder deleted")
            if os.path.exists(obj_path):
                shutil.rmtree(obj_path, ignore_errors=True)
                log("[OK] obj folder deleted")
        except Exception as e:
            log(f"[ERROR] Error cleaning bin/obj folders: {str(e)}")
            return

        log("[INFO] Building backend (dotnet build)...")
        try:
            result = subprocess.run(['dotnet', 'build'], cwd=backend_path, shell=True, check=True, capture_output=True, text=True)
            log(f"[OK] dotnet build: {result.stdout.strip()}")
        except subprocess.CalledProcessError as e:
            log(f"[ERROR] Backend build error: {e.stderr.strip()}")
            return

        log("[INFO] Installing frontend dependencies...")
        try:
            result = subprocess.run(["npm", "install"], cwd=frontend_path, shell=True, check=True, capture_output=True, text=True)
            log(f"[OK] npm install: {result.stdout.strip()}")
        except subprocess.CalledProcessError as e:
            log(f"[ERROR] npm install error: {e.stderr.strip()}")
            return

        output_queue = queue.Queue()

        backend_thread = threading.Thread(
            target=run_process,
            args=(['dotnet', 'run'], backend_path, "Backend", output_queue)
        )
        backend_thread.daemon = True
        backend_thread.start()

        frontend_thread = threading.Thread(
            target=run_process,
            args=(['npm', 'run', 'dev'], frontend_path, "Frontend", output_queue)
        )
        frontend_thread.daemon = True
        frontend_thread.start()

        log("[INFO] Waiting for server and client to start...")
        backend_url = "http://localhost:5000"
        frontend_url = "http://localhost:5176"
        backend_started = False
        frontend_started = False
        start_time = time.time()

        while time.time() - start_time < 30: 
            try:
                while not output_queue.empty():
                    message = output_queue.get_nowait()
                    log(message)
                    if not backend_started and "Now listening on:" in message:
                        backend_url = message.split("Now listening on: ")[1].strip()
                        backend_started = True
                    if not frontend_started and "Local:" in message:
                        frontend_url = message.split("Local:")[1].strip()
                        frontend_started = True
                if backend_started and frontend_started:
                    break
                time.sleep(0.1)
            except queue.Empty:
                time.sleep(0.1)

        log(f"[INFO] Server available at: {backend_url}")
        log(f"[INFO] Client available at: {frontend_url}")
        log("[OK] Successfully started! Press Ctrl+C to terminate.")

        try:
            while True:
                try:
                    while not output_queue.empty():
                        log(output_queue.get_nowait())
                    time.sleep(0.1)
                except queue.Empty:
                    time.sleep(0.1)
        except KeyboardInterrupt:
            log("[INFO] Shutting down...")
            backend_thread.join(timeout=1)
            frontend_thread.join(timeout=1)
            return

    except Exception as e:
        log(f"[ERROR] Error: {str(e)}")

if __name__ == "__main__":
    main()