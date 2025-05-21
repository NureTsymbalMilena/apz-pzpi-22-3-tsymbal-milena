import './Header.css'
import {Link, useNavigate} from 'react-router-dom';
import Logo from '../../assets/logo.png'
import axios from "axios";
import toast from "react-hot-toast";
import '../../i18n';
import { useTranslation } from 'react-i18next';

function Header() {
    const { i18n } = useTranslation();
    const { t } = useTranslation();
    const userId = localStorage.getItem('userId');
    const role = localStorage.getItem('role');
    const navigate = useNavigate()

    const handleLanguageChange = (e) => {
        i18n.changeLanguage(e.target.value);
    };

    const handleLogout = async (e) => {

        try {
            const response = await axios.post('http://localhost:5119/api/Auth/logout');
            console.log(response)
            localStorage.removeItem('userId');
            localStorage.removeItem('role');
            toast.success(t('error.youLoggedOutSuccessfully'))
            navigate("/")
            window.location.reload();
        } catch (error) {
            toast.error(error.response.data.message)
        }
    };

    return (
        <div className='containerHeader'>
            <div className='logoAndNameContainer'>
                <img src={Logo} className="logo" alt=""/>
                <Link className="name" to="/">InRoom</Link>
            </div>
            <div className='links'>
                <select
                    className='language'
                    value={i18n.language}
                    onChange={handleLanguageChange}
                >
                    <option value="en">English</option>
                    <option value="ua">Українська</option>
                </select>
                {role === "PlatformAdmin" && (
                    <Link className="link" to="/change-role">{t('header.changeRole')}</Link>
                )}

                {role === "DatabaseAdmin" && (
                    <Link className="link" to="/backup">{t('header.backup')}</Link>
                )}

                {
                    userId ? (
                        <>
                            <Link className="link" to="/profile">{t('header.profile')}</Link>
                            <p className="link" onClick={handleLogout}>{t('header.logout')}</p>
                        </>
                    ) : (
                        <Link className="link" to="/login">{t('header.login')}</Link>
                    )
                }
            </div>
        </div>
    )
}

export default Header
