import React, { useState, useEffect } from 'react';
import DeleteStaffForm from '../../../Components/StaffProfile/DeleteStaffForm';
import fetchApi from '../../../Services/fetchApi';
import './styles/DeleteStaffPage.css';

const DeleteStaffPage = () => {
    const [staffList, setStaffList] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    useEffect(() => {
        const fetchStaff = async () => {
            setLoading(true);
            setError(null);

            try {
                const data = await fetchApi('/staff/all');
                setStaffList(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchStaff();
    }, []);

    const handleDeactivate = async (staffId) => {
        const isConfirmed = window.confirm("Are you sure you want to deactivate this staff?");
        if (!isConfirmed) return;

        setLoading(true);
        setError(null);
        setSuccess(null);

        try {
            await fetchApi(`/staff/${staffId}`, {
                method: 'DELETE',
            });

            setSuccess('Staff successfully deactivated.');
            setStaffList(staffList.filter(staff => staff.id !== staffId));
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h1>Deactivate Staff</h1>
            {loading && <p>Loading...</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {success && <p style={{ color: 'green' }}>{success}</p>}

            <DeleteStaffForm staffList={staffList} onDelete={handleDeactivate} />
        </div>
    );
};

export default DeleteStaffPage;