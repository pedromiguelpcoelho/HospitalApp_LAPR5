import React, { useState, useEffect } from 'react';
import OperationTypeDetailsPage from './OperationTypeDetailsPage';
import OperationTypesList from '../../../Components/OperationTypes/OperationTypesList';
import fetchApi from '../../../Services/fetchApi';
import '../../../Components/OperationTypes/OperationTypeList.css';

const DeactivateOperationTypesPage = () => {
    const [operationTypes, setOperationTypes] = useState([]);
    const [selectedOperationType, setSelectedOperationType] = useState(null);
    const [staffMembers, setStaffMembers] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [successMessage, setSuccessMessage] = useState(null);
    const [showActive] = useState(true);

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
    };

    const deactivateOperationType = async (id) => {
        setLoading(true);
        setError(null);
        setSuccessMessage(null);

        try {
            await fetchApi(`/operationTypes/${id}`, {
                method: 'DELETE',
            });
            setSuccessMessage("Operation type deactivated successfully!");
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

    const handleBackToList = () => {
        setSelectedOperationType(null);
    };

    const filteredOperationTypes = operationTypes.filter(type => type.isActive === showActive);

    return (
        <div>
            <h1>Deactivate Operation Types</h1>
            {filteredOperationTypes.length > 0 && (
                <p>Click on an operation type to deactivate.</p>
            )}

            {/* Exibir a mensagem de sucesso quando a operação for desativada */}
            {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}

            {selectedOperationType ? (
                <div>
                    <OperationTypeDetailsPage 
                        operationType={selectedOperationType} 
                        staffMembers={staffMembers} 
                        onBack={handleBackToList} // Passa a função handleBackToList como onBack
                    />
                    <button onClick={() => deactivateOperationType(selectedOperationType.id)} style={{ color: 'red' }}>
                        Deactivate
                    </button>
                    <button onClick={handleBackToList}>Back to List</button>
                </div>
            ) : (
                <>
                    {filteredOperationTypes.length > 0 ? (
                        <OperationTypesList 
                            operationTypes={filteredOperationTypes} 
                            onSelect={handleSelectOperationType}
                        />
                    ) : (
                        <p>No active operation types available to deactivate.</p>
                    )}
                </>
            )}
            {loading && <p>Loading...</p>}
            {error && <p style={{ color: 'red' }}>Error: {error}</p>}
        </div>
    );
};

export default DeactivateOperationTypesPage;