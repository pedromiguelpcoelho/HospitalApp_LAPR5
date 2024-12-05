import React, { useState } from 'react';
import DeletePatientProfileForm from '../../../Components/PatientProfile/DeletePatientProfileForm';
import fetchApi from '../../../Services/fetchApi';
import './DeletePatientProfilePage.css';

function DeletePatientProfilePage() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    const handleDelete = async (searchParams) => {
        const isConfirmed = window.confirm("Are you sure you want to delete this patient profile?");
        if (!isConfirmed) {
            return;
        }

        setLoading(true);
        setError(null);
        setSuccess(null);

        try {
            const queryParams = new URLSearchParams(searchParams).toString();
            const data = await fetchApi(`/PatientProfile/delete?${queryParams}`, {
                method: 'DELETE',
            });

            setSuccess(data.message);
            alert("Profile deleted successfully!");
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="delete-patient-profile-container">
            <h1>Delete Patient Profile</h1>
            {error && <p className="error-message">{error}</p>}
            {success && <p className="success-message">{success}</p>}

            <DeletePatientProfileForm onDelete={handleDelete} />

            {loading && <p>Loading...</p>}
        </div>
    );
}

export default DeletePatientProfilePage;