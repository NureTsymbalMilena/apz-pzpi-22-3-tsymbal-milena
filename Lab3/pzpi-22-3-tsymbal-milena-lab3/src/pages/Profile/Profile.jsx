import './Profile.css'
import React, { useState, useEffect } from 'react';
import toast from "react-hot-toast";
import axios from 'axios';
import {useNavigate} from "react-router-dom";
import '../../i18n';
import { useTranslation } from 'react-i18next';

function Profile() {
    const { t } = useTranslation();
    const [userData, setUserData] = useState(null);
    const [hospitalData, setHospitalData] = useState(null);
    const [diseaseData, setDiseaseData] = useState(null);
    const navigate = useNavigate()

    useEffect(() => {
        const fetchUserInfo = async () => {
            const storedUserId = localStorage.getItem('userId');
            if (!storedUserId) {
                toast.error(t('error.errorFetchingUserID'));
            }

            try {
                const response = await axios.get(
                    `http://localhost:5119/api/User/${storedUserId}`,
                    { withCredentials: true }
                );
                setUserData(response.data);
            } catch (error) {
                toast.error(t('error.errorFetchingUserInfo'));
                console.error(error);
            }
        };

        fetchUserInfo();
    }, []);

    useEffect(() => {
        const fetchHospitalInfo = async () => {
            if (!userData?.hospitalId) return;
            try {
                const response = await axios.get(
                    `http://localhost:5119/api/Hospital/${userData.hospitalId}`,
                    {
                        headers: {
                            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
                        },
                        withCredentials: true
                    }
                );
                setHospitalData(response.data);
            } catch (error) {
                toast.error(t('error.errorFetchingHospitalInfo'));
                console.error(error);
            }
        };

        const fetchDiseaseInfo = async () => {
            if (!userData?.diseaseId) return;
            try {
                const response = await axios.get(
                    `http://localhost:5119/api/Disease/${userData.diseaseId}`,
                    {
                        headers: {
                            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
                        },
                        withCredentials: true
                    }
                );
                setDiseaseData(response.data);
            } catch (error) {
                toast.error(t('error.errorFetchingDiseaseInfo'));
                console.error(error);
            }
        };

        if (userData) {
            fetchHospitalInfo();
            fetchDiseaseInfo();
        }
    }, [userData]);

    const formatDate = (dateString) => {
        if (!dateString) return '—';
        const date = new Date(dateString);
        return date.toLocaleString();
    };

    const handleDeleteUser = async () => {
        try {
            await axios.delete(
                `http://localhost:5119/api/User/${userData.userId}`,
                {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
                    },
                    withCredentials: true
                }
            );
            await axios.post('http://localhost:5119/api/Auth/logout');

            localStorage.removeItem('userId');
            localStorage.removeItem('role');

            toast.error(t('error.yourAccountDeletedSuccessfully'));
            navigate("/")
        } catch (error) {
            toast.error(t('error.errorDeletingUser'));
            console.error(error);
        }
    };

    if (!userData) return null;

    return (
        <div className="profileWrapper">
            <h2 className="title">{t('profile.title')}</h2>
            <div className="profileContainer">
                <div className="profileItems">
                    <div className='homeInfoItem'>
                        <p className="homeText">{t('profile.name')}:</p>
                        <p>{userData?.name || '—'} {userData?.surname || '—'}</p>
                    </div>
                    <div className='homeInfoItem'>
                        <p className="homeText">{t('profile.email')}:</p>
                        <p>{userData?.email || '—'}</p>
                    </div>
                    <div className='homeInfoItem'>
                        <p className="homeText">{t('profile.registrationDate')}:</p>
                        <p>{formatDate(userData.createdAt)}</p>
                    </div>
                    <div className='homeInfoItem'>
                        <p className="homeText">{t('profile.role')}:</p>
                        <p>{userData?.role || '—'}</p>
                    </div>
                    <div className='homeInfoItem'>
                        <p className="homeText">{t('profile.hospitalName')}:</p>
                        <p>{hospitalData?.name || '—'}</p>
                    </div>
                    <div className='homeInfoItem'>
                        <p className="homeText">{t('profile.disease')}:</p>
                        <p>{diseaseData?.name || 'None'}</p>
                    </div>
                    <div className='homeInfoItem'>
                        <p className="homeText">{t('profile.isContagious')}:</p>
                        <p>{diseaseData?.isContagious ? 'Yes' : 'No'}</p>
                    </div>
                </div>
                <div className='buttons'>
                    <button className="editButton" onClick={() => navigate("/edit-profile", { state: { userData } })}>{t('profile.edit')}</button>
                    <button onClick={handleDeleteUser}>{t('profile.delete')}</button>
                </div>
            </div>
        </div>
    )
}

export default Profile