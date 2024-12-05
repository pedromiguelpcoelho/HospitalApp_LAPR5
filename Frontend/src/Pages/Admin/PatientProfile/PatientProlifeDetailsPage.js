import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import fetchApi from '../../../Services/fetchApi';

const PatientProfileDetailsPage = () => {
    const { id } = useParams();
    const [patientProfile, setPatientProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchPatientProfile = async () => {
            try {
                const data = await fetchApi(`/patientProfiles/id/${id}`);
                setPatientProfile(data);
            } catch (error) {
                console.error('Error fetching patient profile:', error);
                setError('Failed to fetch patient profile. Please try again later.');
            } finally {
                setLoading(false);
            }
        };

        fetchPatientProfile();
    }, [id]);

    if (loading) {
        return <div>Loading patient profile details...</div>;
    }

    if (error) {
        return <div style={{ color: 'red' }}>{error}</div>;
    }

    return (
        <div>
            <h1>Patient Profile Details</h1>
            {patientProfile ? (
                <div>
                    <p><strong>First Name:</strong> {patientProfile.firstName}</p>
                    <p><strong>Last Name:</strong> {patientProfile.lastName}</p>
                    <p><strong>Full Name:</strong> {patientProfile.fullName}</p>
                    <p><strong>Date of Birth:</strong> {patientProfile.dateOfBirth}</p>
                    <p><strong>Email:</strong> {patientProfile.email}</p>
                    <p><strong>Contact Information:</strong> {patientProfile.contactInformation}</p>
                    <button onClick={() => navigate('/admin/getpatientprofiles')}>Back to List</button>
                </div>
            ) : (
                <div>No patient profile found.</div>
            )}
        </div>
    );
};

export default PatientProfileDetailsPage;