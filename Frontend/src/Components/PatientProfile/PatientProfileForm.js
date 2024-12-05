import React, { useState } from 'react';

function PatientProfileForm({ onCreateProfile }) {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [fullName, setFullName] = useState('');
    const [dateOfBirth, setDateOfBirth] = useState('');
    const [email, setEmail] = useState('');
    const [contactInformation, setContactInformation] = useState('');
    const [error, setError] = useState(''); // eslint-disable-line no-unused-vars

    const handleSubmit = () => {
        // Reset error message
        setError('');

        // Validate inputs
        if (!firstName || !lastName || !fullName || !dateOfBirth || !email || !contactInformation) {
            setError('All fields are required.');
            return;
        }

        // Prepare data
        const data = {
            firstName,
            lastName,
            fullName,
            dateOfBirth,
            email,
            contactInformation,
        };

        // Call the onCreateProfile function passed from the parent
        onCreateProfile(data);

        // Reset form fields
        setFirstName('');
        setLastName('');
        setFullName('');
        setDateOfBirth('');
        setEmail('');
        setContactInformation('');
    };

    return (
        <div id="profile-form">
            <h2>Add Patient Profile</h2>
            <input
                type="text"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                placeholder="First Name"
                required
            />
            <input
                type="text"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                placeholder="Last Name"
                required
            />
            <input
                type="text"
                value={fullName}
                onChange={(e) => setFullName(e.target.value)}
                placeholder="Full Name"
                required
            />
            <input
                type="date"
                value={dateOfBirth}
                onChange={(e) => setDateOfBirth(e.target.value)}
                placeholder="Date of Birth"
                required
            />
            <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="Email"
                required
            />
            <input
                type="text"
                value={contactInformation}
                onChange={(e) => setContactInformation(e.target.value)}
                placeholder="Contact Information"
                required
            />
            <button onClick={handleSubmit}>
                Create
            </button>
        </div>
    );
}

export default PatientProfileForm;