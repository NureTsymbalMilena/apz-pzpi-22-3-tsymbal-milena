import './Login.css'
import { useNavigate, Link } from 'react-router-dom';
import { useState } from 'react';
import toast from "react-hot-toast";
import axios from 'axios';
import '../../i18n';
import { useTranslation } from 'react-i18next';

function Login() {
  const { t } = useTranslation();
  const [formData, setFormData] = useState({
    Email: '',
    Password: '',
  });

  const navigate = useNavigate()

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('http://localhost:5119/api/Auth/login', formData);
      console.log(response)
      const { userId, role, accessToken } = response.data;
      localStorage.setItem('userId', userId);
      localStorage.setItem('role', role);
      localStorage.setItem('accessToken', accessToken);
      toast.success(t('error.youLoggedInSuccessfully'))
      navigate("/")

    } catch (error) {
      toast.error(error.response.data.message)
    }
  };

  return (
    <div className="wrapper">
      <div className="container">
        <form onSubmit={handleSubmit}>
          <h2 className="title">{t('login.title')}</h2>
          <div className="formGroup">
            <label htmlFor="Email">{t('login.email')}</label>
            <input type="email" id="Email" name="Email" placeholder={t('login.emailPlaceholder')} value={formData.Email}
                   onChange={handleChange} required/>
          </div>
          <div className="formGroup">
            <label htmlFor="Password">{t('login.password')}</label>
            <input type="password" id="Password" name="Password" placeholder={t('login.passwordPlaceholder')} value={formData.Password}
                   onChange={handleChange} required/>
          </div>
          <button type="submit">{t('login.title')}</button>
          <p className='dontHaveAndAccount'>
            {t('login.dontHaveAnAccount')}
            <Link className="link register" to="/register">{t('login.register')}</Link>
          </p>
        </form>
      </div>
    </div>
  )
}

export default Login