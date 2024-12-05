import React, { useState } from 'react';
import PatientProfileForm from '../../../Components/PatientProfile/PatientProfileForm';
import fetchApi from '../../../Services/fetchApi';

function AddPatientProfilePage() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null); // eslint-disable-line no-unused-vars
    const [success, setSuccess] = useState(null); // eslint-disable-line no-unused-vars

    const createPatientProfile = async (data) => {
        setLoading(true);
        setError(null);
        setSuccess(null);

        try {
            const response = await fetchApi('/PatientProfile', {
                method: 'POST',
                body: JSON.stringify(data),
            });

            if (response.status === 409) {
                setError(response.message);
            } else {
                setSuccess("Patient profile created successfully!");
            }
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <PatientProfileForm onCreateProfile={createPatientProfile} />
            {loading && <p>Creating...</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {success && <p style={{ color: 'green' }}>{success}</p>}        
        </div>
    );
}

export default AddPatientProfilePage;