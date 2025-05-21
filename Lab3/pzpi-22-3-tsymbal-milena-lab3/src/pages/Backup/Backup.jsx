import './Backup.css'
import axios from 'axios';
import React, {useState} from 'react';
import toast from "react-hot-toast";
import '../../i18n';
import { useTranslation } from 'react-i18next';

function Backup() {
    const { t } = useTranslation();
    const [backupPath, setBackupPath] = useState('');
    const [restoreFilePath, setRestoreFilePath] = useState('');

    const handleBackupSubmit = async (e) => {
        e.preventDefault();
        try {
            const formData = new FormData();
            formData.append("outputDirectory", backupPath);

            const response = await axios.post(
                `http://localhost:5119/api/Admin/backup`,
                formData,
                {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
                    },
                    withCredentials: true
                }
            );
            toast.success(response.data.message);
            setBackupPath('');
        } catch (error) {
            toast.error(error?.response?.data?.message || t('error.backupFailed'));
        }
    };

    const handleRestoreSubmit = async (e) => {
        e.preventDefault();

        if (!restoreFilePath) {
            toast.error(t('error.pleasSelectABakFile'));
            return;
        }

        try {
            const response = await axios.post(
                `http://localhost:5119/api/Admin/restore`,
                JSON.stringify(restoreFilePath),
                {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
                        'Content-Type': 'application/json',
                    },
                    withCredentials: true
                }
            );
            toast.success(response.data.message);
            setRestoreFilePath('');
        } catch (error) {
            toast.error(error?.response?.data?.message || t('error.restoreFailed'));
        }
    };

    return (
        <div className="wrapper">
            <div className="container backup">
                <form onSubmit={handleBackupSubmit}>
                    <h2 className="title">{t('backup.backupTitle')}</h2>
                    <div className='formItems'>
                        <div className="formGroup">
                            <label htmlFor="backupPath">{t('backup.backupDirectory')}</label>
                            <input
                                type="text"
                                id="backupPath"
                                name="backupPath"
                                placeholder={t('backup.backupPlaceholder')}
                                value={backupPath}
                                onChange={(e) => setBackupPath(e.target.value)}
                                required
                            />
                        </div>
                    </div>
                    <button type="submit">{t('backup.backupTitle')}</button>
                </form>
            </div>

            <div className="container">
                <form onSubmit={handleRestoreSubmit}>
                    <h2 className="title">{t('backup.restoreTitle')}</h2>
                    <div className='formItems'>
                        <div className="formGroup">
                            <label htmlFor="restoreFilePath">{t('backup.selectBakFile')}</label>
                            <input
                                type="text"
                                id="restoreFilePath"
                                placeholder={t('backup.restorePlaceholder')}
                                value={restoreFilePath}
                                onChange={(e) => setRestoreFilePath(e.target.value)}
                                required
                            />
                        </div>
                    </div>
                    <button type="submit">{t('backup.restoreTitle')}</button>
                </form>
            </div>
        </div>
    );
}

export default Backup;