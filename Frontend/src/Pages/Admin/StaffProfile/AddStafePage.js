import React, { useState } from 'react';
import StaffForm from '../../../Components/StaffProfile/StaffForm';
import fetchApi from '../../../Services/fetchApi';
import './styles/AddStafePage.css';

function AddStaffPage() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [successMessage, setSuccessMessage] = useState('');
    const [staffData, setStaffData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        role: '',
        specialization: '',
    });

    // Handle form input changes
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setStaffData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    // Handle staff member creation
    const addStaffMember = async (staffData) => {
        setLoading(true);
        setError(null);
        setSuccessMessage('');

        try {
            await fetchApi('/staff', {
                method: 'POST',
                body: JSON.stringify(staffData),
            });

            setSuccessMessage('Staff member added successfully!');
            setStaffData({
                firstName: '',
                lastName: '',
                email: '',
                phoneNumber: '',
                role: '',
                specialization: '',
            });
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h1>Add New Staff</h1>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}

            <StaffForm
                onCreateStaffMember={addStaffMember}
                staffData={staffData}
                handleInputChange={handleInputChange}
            />

            {loading && <p>Adding Staff...</p>}
        </div>
    );
}

export default AddStaffPage;