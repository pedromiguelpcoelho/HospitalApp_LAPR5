import React, { useState } from 'react';
import UpdateMyOwnPatientProfileForm from '../../Components/PatientProfile/UpdateMyOwnPatientProfileForm';
import fetchApi from '../../Services/fetchApi';

const UpdateMyOwnPatientProfilePage = () => {
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        fullName: '',
        dateOfBirth: '',
        email: '',
        contactInformation: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const userConfirmed = window.confirm('Do you accept the treatment and processing of your personal data by the Wonderful Hospital application?');
        if (!userConfirmed) {
            return;
        }
        try {
            await fetchApi('/PatientProfile/updatePatient', {
                method: 'PUT',
                body: JSON.stringify(formData)
            });
            alert('Profile updated successfully');
        } catch (error) {
            console.error('There was an error updating the profile!', error);
            alert('Failed to update profile');
        }
    };

    return (
        <div>
            <h1>Update My Profile</h1>
            <UpdateMyOwnPatientProfileForm formData={formData} handleChange={handleChange} handleSubmit={handleSubmit} />
        </div>
    );
};

export default UpdateMyOwnPatientProfilePage;