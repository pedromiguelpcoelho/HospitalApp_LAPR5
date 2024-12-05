import React, { useState, useEffect } from 'react';
import Cookies from "js-cookie";

function DeletePatientProfileForm({ onDelete }) {
    const [selectedParam, setSelectedParam] = useState('');
    const [inputValue, setInputValue] = useState('');
    const [profileDetails, setProfileDetails] = useState(null);

    const handleParamChange = (e) => {
        setSelectedParam(e.target.value);
        setInputValue('');
        setProfileDetails(null);
    };

    const handleInputChange = (e) => {
        setInputValue(e.target.value);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onDelete({ [selectedParam]: inputValue });
    };

    useEffect(() => {
        const fetchProfileDetails = async () => {
            if (selectedParam && inputValue) {
                try {
                    const token = Cookies.get('token');
                    const queryParams = new URLSearchParams({ [selectedParam]: inputValue }).toString();
                    const response = await fetch(`http://localhost:5002/api/PatientProfile/all?${queryParams}`, {
                        method: 'GET',
                        headers: {
                            'Authorization': `Bearer ${token}`,
                            'Content-Type': 'application/json'
                        }
                    });

                    if (response.ok) {
                        const data = await response.json();
                        setProfileDetails(data[0]); // Assuming the API returns an array of profiles
                    } else {
                        setProfileDetails(null);
                    }
                } catch (error) {
                    console.error('Error fetching profile details:', error);
                    setProfileDetails(null);
                }
            }
        };

        fetchProfileDetails();
    }, [selectedParam, inputValue]);

    return (
        <form onSubmit={handleSubmit}>
            <select value={selectedParam} onChange={handleParamChange}>
                <option value="">Select Parameter</option>
                <option value="fullName">Full Name</option>
                <option value="medicalRecordNumber">Medical Record Number</option>
                <option value="dateOfBirth">Date of Birth</option>
                <option value="email">Email</option>
                <option value="phoneNumber">Phone Number</option>
            </select>

            {selectedParam && (
                <input
                    type={selectedParam === 'dateOfBirth' ? 'date' : 'text'}
                    name={selectedParam}
                    placeholder={selectedParam.charAt(0).toUpperCase() + selectedParam.slice(1).replace(/([A-Z])/g, ' $1')}
                    value={inputValue}
                    onChange={handleInputChange}
                />
            )}

            {profileDetails && (
                <div className="profile-details">
                    <p><strong>Full Name:</strong> {profileDetails.fullName}</p>
                    <p><strong>Email:</strong> {profileDetails.email}</p>
                    <p><strong>Medical Record Number:</strong> {profileDetails.medicalRecordNumber}</p>
                </div>
            )}

            <button type="submit">Delete Profile</button>
        </form>
    );
}

export default DeletePatientProfileForm;