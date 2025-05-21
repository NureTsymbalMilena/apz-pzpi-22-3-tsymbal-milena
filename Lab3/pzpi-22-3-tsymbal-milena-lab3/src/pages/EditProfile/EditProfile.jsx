import './EditProfile.css'
import axios from 'axios';
import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import toast from "react-hot-toast";
import '../../i18n';
import { useTranslation } from 'react-i18next';

function EditProfile() {
    const { t } = useTranslation();
    const location = useLocation();
    const navigate = useNavigate();
    const userData = location.state?.userData;

    const [formData, setFormData] = useState({
        Name: userData.name || '',
        Surname: userData.surname || '',
        Email: userData.email || '',
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const userId = localStorage.getItem('userId');
            await axios.patch(`http://localhost:5119/api/User/${userId}`, formData);
            toast.success(t('error.youEditedProfileSuccessfully'))
            navigate("/profile")
        } catch (error) {
            toast.error(error.response.data.message)
        }
    };

    return (
        <div className="wrapper">
            <div className="container">
                <form onSubmit={handleSubmit}>
                    <h2 className="title">{t('editProfile.title')}</h2>
                    <div className='formItems'>
                        <div className="formGroup">
                            <label htmlFor="Name">{t('editProfile.name')}</label>
                            <input type="text" id="Name" name="Name" placeholder={t('editProfile.namePlaceholder')} value={formData.Name}
                                   onChange={handleChange} required/>
                        </div>
                        <div className="formGroup">
                            <label htmlFor="Surname">{t('editProfile.surname')}</label>
                            <input type="text" id="Surname" name="Surname" placeholder={t('editProfile.surnamePlaceholder')} value={formData.Surname}
                                   onChange={handleChange} required/>
                        </div>
                        <div className="formGroup">
                            <label htmlFor="Email">{t('editProfile.email')}</label>
                            <input type="email" id="Email" name="Email" placeholder={t('editProfile.emailPlaceholder')} value={formData.Email}
                                   onChange={handleChange} required/>
                        </div>
                    </div>
                    <button type="submit">{t('editProfile.button')}</button>
                </form>
            </div>
        </div>
    )
}

export default EditProfile
