import React, { useState, useEffect } from 'react';
import UpdateOperationTypeForm from '../../../Components/OperationTypes/UpdateOperationTypeForm';
import OperationTypesList from '../../../Components/OperationTypes/OperationTypesList';
import fetchApi from '../../../Services/fetchApi';
import '../../../Components/OperationTypes/AddOperationTypeForm.css';
import '../../../Components/OperationTypes/OperationTypeList.css';

const UpdateOperationTypePage = () => {
    const [operationTypes, setOperationTypes] = useState([]);
    const [selectedOperationType, setSelectedOperationType] = useState(null);
    const [staffMembers, setStaffMembers] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [showForm, setShowForm] = useState(false);

    useEffect(() => {
        const fetchOperationTypes = async () => {
            try {
                const data = await fetchApi('/operationTypes/all');
                setOperationTypes(data);
            } catch (error) {
                setError(error.message);
            }
        };

        const fetchStaffMembers = async () => {
            try {
                const data = await fetchApi('/staff/all');
                setStaffMembers(data);
            } catch (error) {
                setError(error.message);
            }
        };

        fetchOperationTypes();
        fetchStaffMembers();
    }, []);

    const handleSelectOperationType = (operationType) => {
        setSelectedOperationType(operationType);
        setShowForm(true);
    };

    const updateOperationType = async (id, updatedData) => {
        setLoading(true);
        setError(null);

        try {
            await fetchApi(`/operationTypes/${id}`, {
                method: 'PUT',
                body: JSON.stringify(updatedData),
            });
            alert("Operation type updated successfully!");
            setShowForm(false);
            setSelectedOperationType(null);
            // Refresh the list of operation types
            const data = await fetchApi('/operationTypes/all');
            setOperationTypes(data);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    //const activeOperationTypes = operationTypes.filter(type => type.isActive);
    const activeOperationTypes = (operationTypes || []).filter(type => type.isActive);


    return (
        <div>
            <h1>Update Operation Type</h1>
            {!showForm && (
                <p>
                    {activeOperationTypes.length > 0
                        ? "Click on an operation type to update its details."
                        : "No active operation types available to update."}
                </p>
            )}
            {showForm && selectedOperationType ? (
                <UpdateOperationTypeForm 
                    operationType={selectedOperationType}
                    staffMembers={staffMembers}
                    onBack={() => setShowForm(false)}
                    onUpdate={updateOperationType}
                />
            ) : (
                activeOperationTypes.length > 0 && (
                    <OperationTypesList 
                        operationTypes={activeOperationTypes} 
                        onSelect={handleSelectOperationType}
                    />
                )
            )}
            {loading && <p>Loading...</p>}
            {error && <p className="add-operation-type-form-error">Error: {error}</p>}
        </div>
    );
};

export default UpdateOperationTypePage;