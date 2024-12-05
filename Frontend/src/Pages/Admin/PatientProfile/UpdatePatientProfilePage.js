import React, { useState } from 'react';
import UpdatePatientProfileForm from '../../../Components/PatientProfile/UpdatePatientProfileForm';
import fetchApi from '../../../Services/fetchApi';
import './UpdatePatientProfilePage.css';

function UpdatePatientProfilePage() {
    const [searchParams, setSearchParams] = useState({
        email: '',
        medicalRecordNumber: '',
        fullName: '',
        dateOfBirth: '',
        phoneNumber: ''
    });
    const [selectedFilter, setSelectedFilter] = useState('');
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        fullName: '',
        email: '',
        contactInformation: '',
        dateOfBirth: '',
        medicalRecordNumber: ''
    });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    const handleSearchChange = (e) => {
        const { name, value } = e.target;
        setSearchParams(prevParams => ({
            ...prevParams,
            [name]: value
        }));
    };

    const handleFilterChange = (e) => {
        setSelectedFilter(e.target.value);
        setSearchParams({
            email: '',
            medicalRecordNumber: '',
            fullName: '',
            dateOfBirth: '',
            phoneNumber: ''
        });
    };

    const handleFormChange = (e) => {
        const { name, value } = e.target;
        setFormData(prevData => ({
            ...prevData,
            [name]: value
        }));
    };

    const handleSearch = async () => {
        setLoading(true);
        setError(null);
        setSuccess(null);

        try {
            const queryParams = new URLSearchParams({ [selectedFilter]: searchParams[selectedFilter] }).toString();
            const data = await fetchApi(`/PatientProfile/all?${queryParams}`, {
                method: 'GET',
            });

            if (data.length === 0) {
                throw new Error('No patient profile found.');
            }

            const profile = data[0];
            setFormData({
                firstName: profile.firstName,
                lastName: profile.lastName,
                fullName: profile.fullName,
                email: profile.email,
                contactInformation: profile.contactInformation,
                dateOfBirth: profile.dateOfBirth.split('T')[0], 
                medicalRecordNumber: profile.medicalRecordNumber
            });
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    const handleUpdate = async () => {
        if (!window.confirm(`Are you sure you want to update the profile of ${formData.fullName}?`)) {
            return;
        }
    
        setLoading(true);
        setError(null);
        setSuccess(null);
    
        try {
            const queryParams = new URLSearchParams(
                Object.fromEntries(Object.entries(searchParams).filter(([_, value]) => value))
            ).toString();
            await fetchApi(`/PatientProfile/update?${queryParams}`, {
                method: 'PUT',
                body: JSON.stringify(formData),
            });
    
            setSuccess('Patient profile updated successfully.');
            alert('Patient profile updated successfully. The patient will be informed about this change.');
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };
    
    return (
        <div className="update-patient-profile-container">
            <h1>Update Patient Profile</h1>
            {error && <p className="error-message">{error}</p>}
            {success && <p className="success-message">{success}</p>}
    
            <UpdatePatientProfileForm
                searchParams={searchParams}
                selectedFilter={selectedFilter}
                onSearchChange={handleSearchChange}
                onFilterChange={handleFilterChange}
                onFormChange={handleFormChange}
                onSearch={handleSearch}
                onUpdate={handleUpdate}
                formData={formData}
                loading={loading}
            />
        </div>
    );
}

export default UpdatePatientProfilePage;