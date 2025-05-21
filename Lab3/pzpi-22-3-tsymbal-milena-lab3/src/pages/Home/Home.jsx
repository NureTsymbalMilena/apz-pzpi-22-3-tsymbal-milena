import './Home.css'
import { useState, useEffect } from 'react';
import toast from "react-hot-toast";
import axios from 'axios';
import '../../i18n';
import i18n from 'i18next';
import { useTranslation } from 'react-i18next';

function Home() {
    const { t } = useTranslation();
    const [analysisData, setAnalysisData] = useState(null);
    const [userId, setUserId] = useState(null);

    useEffect(() => {
    const fetchAnalysis = async () => {
        const storedUserId = localStorage.getItem('userId');
        if (!storedUserId) {
            setUserId(null);
            return;
        }

        setUserId(storedUserId);

        try {

        const response = await axios.post(
            `http://localhost:5119/api/Contact/${storedUserId}/epidemiological-risk-analysis`,
            {}, 
            { withCredentials: true }
        );

        setAnalysisData(response.data);
        } catch (error) {
        toast.error(t('error.errorFetchingHospitals'));
        console.error(error);
        }
    };

    fetchAnalysis();
    }, []);

    const formatDuration = (durationString) => {
        if (!durationString) return '—';
        
        const [hours, minutes, rest] = durationString.split(':');
        const seconds = Math.floor(parseFloat(rest));

        const h = parseInt(hours);
        const m = parseInt(minutes);

        return `${h > 0 ? `${h} ${t('analysis.hours')} ` : ''}${m > 0 ? `${m} ${t('analysis.minutes')} ` : ''}${seconds} ${t('analysis.seconds')}`;

    };

    const formatDate = (dateString) => {
        if (!dateString) return '—';
        const date = new Date(dateString);
        const currentLang = i18n.language || 'en';

        return date.toLocaleString(currentLang, {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
        });
    };

    const formatDistance = (meters) => {
        if (meters == null) return '—';

        const currentLang = i18n.language || 'en';

        if (currentLang === 'en' || currentLang.startsWith('en')) {
            const feet = meters * 3.28084;
            return `${feet.toFixed(1)} ft`;
        }

        return `${meters.toFixed(1)} м`;
    };

    if (!userId) return null;

    return (
        <div className="homeWrapper">
            <h2 className="authTitle">{t('analysis.title')}</h2>
            <div className="homeContainer">
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.name')}:</p>
                    <p>{analysisData?.name || '—'} {analysisData?.surname || '—'}</p>
                </div>
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.disease')}:</p>
                    <p>{analysisData?.userDisease?.name || 'None'}</p>
                </div>
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.isContagious')}:</p>
                    <p>{analysisData?.isContagious ? 'Yes' : 'No'}</p>
                </div>
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.totalRisk')}:</p>
                    <p>{analysisData?.totalRisk?.toFixed(2) || '—'}</p>
                </div>
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.riskLevel')}:</p>
                    <p>{analysisData?.riskLevel || '—'}</p>
                </div>
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.averageContactDuration')}:</p>
                    <p>{formatDuration(analysisData?.averageContactDuration)}</p>
                </div>
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.potentialDiseases')}:</p>
                    <p>{analysisData?.potentialDiseases?.join(', ') || 'None'}</p>
                </div>
                <div className='homeInfoItem'>
                    <p className="homeText">{t('analysis.contacts')}:</p>
                    <p>{analysisData?.contacts?.length || 0}</p>
                </div>
            </div>
            {analysisData?.contacts?.length > 0 && (
                <table>
                    <thead>
                        <tr>
                            <th>{t('analysis.name')}</th>
                            <th>{t('analysis.surname')}</th>
                            <th>{t('analysis.disease')}</th>
                            <th>{t('analysis.roomName')}</th>
                            <th>{t('analysis.minimalDistance')}</th>
                            <th>{t('analysis.contactStartTime')}</th>
                            <th>{t('analysis.contactEndTime')}</th>
                        </tr>
                    </thead>
                    <tbody>
                        {analysisData.contacts.map((contact, index) => (
                            <tr key={index}>
                                <td>{contact.userName}</td>
                                <td>{contact.userSurname}</td>
                                <td>{contact.userDisease}</td>
                                <td>{contact.roomName}</td>
                                <td>{formatDistance(contact.minDistance)}</td>
                                <td>{formatDate(contact.contactStartTime)}</td>
                                <td>{formatDate(contact.contactEndTime)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    )
}

export default Home