import './App.css';
import QRCode from './assets/my_qrcode.png';

function App() {
    return (
        <div className="container">
            <div className="desktopContainer">
                <p>
                    1. Install <span className="underline">ZIP file</span>:
                </p>

                <a href="/Installer/InRoom.zip" download>
                    <button type="button" className="btn btn-dark">
                        Download ZIP
                    </button>
                </a>

                <p>
                    2. Install <span className="underline">deploy script</span>:
                </p>

                <a href="/Installer/deploy.py" download>
                    <button type="button" className="btn btn-dark">
                        Download Deploy Script
                    </button>
                </a>

                <p>
                    3. Install <span className="underline">.bat file</span>:
                </p>

                <a href="/Installer/deploy.bat" download>
                    <button type="button" className="btn btn-dark">
                        Download & Run Installer
                    </button>
                </a>

                <p>
                    4. Run <span className="underline">.bat file</span>:
                </p>

            </div>

            <div className="mobileContainer">
                <p>
                    Scan the QR code to install the <span className="underline">mobile app</span>:
                </p>
                <img src={QRCode} className="qrCode" alt="QR Code" />
            </div>
        </div>
    );
}

export default App;