import './Register.css'
import axios from 'axios';
import React, {useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';
import toast from "react-hot-toast";
import '../../i18n';
import { useTranslation } from 'react-i18next';

function Register() {
    const { t } = useTranslation();
    const [formData, setFormData] = useState({
        Name: '',
        Surname: '',
        Email: '',
        Password: '',
        HospitalName: '',
    });
    const [hospitals, setHospitals] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchHospitals = async () => {

            try {

                const response = await axios.get(
                    `http://localhost:5119/api/Hospital`,
                    {},
                    { withCredentials: true }
                );

                setHospitals(response.data);
            } catch (error) {
                toast.error(t('error.errorFetchingHospitals'));
                console.error(error);
            }
        };

        fetchHospitals();
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            await axios.post('http://localhost:5119/api/Auth/register', formData);

            toast.success(t('error.youRegisteredSuccessfully'))
            navigate("/")
        } catch (error) {
            toast.error(error.response.data.message)
        }
    };

    return (
        <div className="wrapper">
            <div className="container">
                <form onSubmit={handleSubmit}>
                    <h2 className="title">{t('register.title')}</h2>
                    <div className='formItems'>
                        <div className="formGroup">
                            <label htmlFor="Name">{t('register.name')}</label>
                            <input type="text" id="Name" name="Name" placeholder={t('register.namePlaceholder')} value={formData.Name}
                                   onChange={handleChange} required/>
                        </div>
                        <div className="formGroup">
                            <label htmlFor="Surname">{t('register.surname')}</label>
                            <input type="text" id="Surname" name="Surname" placeholder={t('register.surnamePlaceholder')} value={formData.Surname}
                                   onChange={handleChange} required/>
                        </div>
                        <div className="formGroup">
                            <label htmlFor="Email">{t('register.email')}</label>
                            <input type="email" id="Email" name="Email" placeholder={t('register.emailPlaceholder')} value={formData.Email}
                                   onChange={handleChange} required/>
                        </div>
                        <div className="formGroup">
                            <label htmlFor="Password">{t('register.password')}</label>
                            <input type="password" id="Password" name="Password" placeholder={t('register.passwordPlaceholder')}
                                   value={formData.Password}
                                   onChange={handleChange} required/>
                        </div>
                        <label htmlFor="dropdown">{t('register.hospitalName')}</label>
                        <select
                            id="HospitalName"
                            name="HospitalName"
                            value={formData.HospitalName}
                            onChange={handleChange}
                            required
                        >
                            <option value="">{t('register.hospitalPlaceholder')}</option>
                            {hospitals.map(hospital => (
                                <option key={hospital.hospitalId} value={hospital.name}>
                                    {hospital.name}
                                </option>
                            ))}
                        </select>
                    </div>
                    <button type="submit">{t('register.title')}</button>
                </form>
            </div>
        </div>
    )
}

export default Register
