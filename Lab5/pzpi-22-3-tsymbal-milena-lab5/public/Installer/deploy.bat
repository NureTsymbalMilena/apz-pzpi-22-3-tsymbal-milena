@echo off

:: --- Проверка прав администратора ---
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo [INFO] Admin right needed. Rerun...
    powershell -Command "Start-Process '%~f0' -Verb runAs"
    exit /b
)

setlocal EnableDelayedExpansion

:: Установить кодировку UTF-8
chcp 65001 >nul

:: Путь к папке, где находится deploy.bat
set "SCRIPT_DIR=%~dp0"
:: Путь к InRoom.zip в той же папке
set "ZIP_PATH=%SCRIPT_DIR%InRoom.zip"
:: Путь к deploy.py
set "DEPLOY_PY=%SCRIPT_DIR%deploy.py"
set "EXTRACT_TO=%USERPROFILE%\Desktop\InRoomProject"

:: Отладка: вывести пути для проверки
echo SCRIPT_DIR=%SCRIPT_DIR%
echo ZIP_PATH=%ZIP_PATH%
echo DEPLOY_PY=%DEPLOY_PY%
echo EXTRACT_TO=%EXTRACT_TO%
echo.
dir "%ZIP_PATH%"
echo.

if not exist "%ZIP_PATH%" (
    echo [ERROR] ZIP-file not found: %ZIP_PATH%
    echo Make sure InRoom.zip is in the same file as Deploy.bat.
    pause
    exit /b 1
)

if not exist "%DEPLOY_PY%" (
    echo [ERROR] File deploy.py not found: %DEPLOY_PY%
    echo Make sure deploy.py is in the same folder as deploy.bat.
    pause
    exit /b 1
)

echo [OK] Архив найден: %ZIP_PATH%

where python >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo [ERROR] Python is not installed. Install Python с https://www.python.org/downloads/
    pause
    exit /b 1
)

where node >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo [ERROR] Node.js is not installed. Install Node.js с https://nodejs.org/
    pause
    exit /b 1
)

where dotnet >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo [ERROR] .NET SDK is not installed. Install .NET SDK 8.0 с https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

python "%DEPLOY_PY%" "%ZIP_PATH%" "%EXTRACT_TO%"
if %ERRORLEVEL% neq 0 (
    echo [ERROR] Error executing deploy.py. Check debug.txt and deploy.log in the folder %SCRIPT_DIR%.
    type "%SCRIPT_DIR%debug.txt"
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [INFO] Check the deploy.log file for details.
echo.
pause
