import React, { useState, useEffect } from 'react';
import fetchApi from '../../../Services/fetchApi';
import OperationTypeForm from '../../../Components/OperationTypes/OperationTypeForm';

function AddOperationTypePage() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [staffByRole, setStaffByRole] = useState({});
    const [formData, setFormData] = useState({
        name: '',
        requiredStaff: [],
        estimatedDuration: '',
    });

    useEffect(() => {
        const fetchStaffMembers = async () => {
            try {
                const data = await fetchApi('/staff/all');
                const groupedByRole = data.reduce((acc, staff) => {
                    const role = staff.role || 'Other';
                    if (!acc[role]) acc[role] = [];
                    acc[role].push(staff);
                    return acc;
                }, {});
                setStaffByRole(groupedByRole);
            } catch (error) {
                setError(error.message);
            }
        };

        fetchStaffMembers();
    }, []);

    const createOperationType = async (formData) => {
        setLoading(true);
        setError(null);

        try {
            await fetchApi('/operationTypes', {
                method: 'POST',
                body: JSON.stringify(formData),
            });
            alert("Operation type created successfully!");
            // Reset formData after successful creation
            setFormData({
                name: '',
                requiredStaff: [],
                estimatedDuration: '',
            });
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    const staffMembers = Object.values(staffByRole).flat();

    return (
        <div>
            <OperationTypeForm 
                onCreate={createOperationType} 
                staffMembers={staffMembers} 
                initialData={formData}
            />
            {loading && <p>Creating...</p>}
            {error && <p className="add-operation-type-form-error">Error: {error}</p>}
        </div>
    );
}
export default AddOperationTypePage;
