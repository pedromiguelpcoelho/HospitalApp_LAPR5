import React, { useState } from 'react';
import RequestDeletionOfPatientProfileForm from '../../Components/PatientProfile/RequestDeletionOfPatientProfileForm';
import fetchApi from '../../Services/fetchApi';

const RequestDeletionOfPatientProfilePage = () => {
    const [message, setMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await fetchApi(`/PatientProfile/request-deletion`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            setMessage('A confirmation email has been sent to your email address.');
        } catch (error) {
            console.error('Request failed:', error);
            setMessage('Failed to request account deletion.');
        }
    };

    return (
        <div>
            <RequestDeletionOfPatientProfileForm handleSubmit={handleSubmit} />
            {message && <p>{message}</p>}
        </div>
    );
};

export default RequestDeletionOfPatientProfilePage;