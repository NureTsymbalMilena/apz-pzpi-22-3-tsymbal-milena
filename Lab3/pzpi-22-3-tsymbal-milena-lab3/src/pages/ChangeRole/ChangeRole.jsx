import './ChangeRole.css'
import React, { useState, useEffect } from 'react';
import toast from "react-hot-toast";
import axios from 'axios';
import '../../i18n';
import { useTranslation } from 'react-i18next';

function ChangeRole() {
    const { t } = useTranslation();
    const [formData, setFormData] = useState({
        UserEmail: '',
        Role: '',
    });
    const [users, setUsers] = useState([]);
    const roles = ['User', 'PlatformAdmin', 'SystemAdmin', 'DatabaseAdmin', 'HospitalAdmin'];

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get('http://localhost:5119/api/User');
                setUsers(response.data);
            } catch (error) {
                toast.error(t('error.errorFetchingUsers'));
                console.error(error);
            }
        };

        fetchUsers();
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
             await axios.patch(
                'http://localhost:5119/api/Admin/role',formData,
                {
                    withCredentials: true,
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            toast.success(t('error.youChangedRoleSuccessfully'));
        } catch (error) {
            toast.error(error.response?.data?.message || t('error.errorChangingRole'));
            console.error(error);
        }
    };

    return (
        <div className="wrapper">
            <div className="container">
                <form onSubmit={handleSubmit}>
                    <h2 className="title">{t('changeRole.title')}</h2>
                    <div className='formItems'>
                        <label htmlFor="Email">{t('changeRole.email')}</label>
                        <select
                            id="UserEmail"
                            name="UserEmail"
                            value={formData.UserEmail}
                            onChange={handleChange}
                            required
                        >
                            <option value="">{t('changeRole.selectAnEmail')}</option>
                            {users.map(user => (
                                <option key={user.userId} value={user.email}>
                                    {user.email}
                                </option>
                            ))}
                        </select>
                        <label htmlFor="Role">{t('changeRole.role')}</label>
                        <select
                            id="Role"
                            name="Role"
                            value={formData.Role}
                            onChange={handleChange}
                            required
                        >
                            <option value="">{t('changeRole.selectARole')}</option>
                            {roles.map(role => (
                                <option key={role} value={role}>
                                    {role}
                                </option>
                            ))}
                        </select>
                    </div>
                    <button type="submit">{t('changeRole.button')}</button>
                </form>
            </div>
        </div>
    )
}

export default ChangeRole