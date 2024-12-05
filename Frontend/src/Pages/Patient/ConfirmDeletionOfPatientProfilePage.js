import React, { useState } from 'react';
import ConfirmDeletionOfPatientProfileForm from '../../Components/PatientProfile/ConfirmDeletionOfPatientProfileForm';
import fetchApi from '../../Services/fetchApi';

const ConfirmDeletionOfPatientProfilePage = () => {
    const [message, setMessage] = useState('');

    const handleSubmit = async (token) => {
        try {
            await fetchApi(`/PatientProfile/confirm-deletion?token=${encodeURIComponent(token)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            setMessage('Account deletion confirmed.');
        } catch (error) {
            console.error('Request failed:', error);
            setMessage('Failed to confirm account deletion.');
        }
    };

    return (
        <div>
            <ConfirmDeletionOfPatientProfileForm handleSubmit={handleSubmit} />
            {message && <p>{message}</p>}
        </div>
    );
};

export default ConfirmDeletionOfPatientProfilePage;