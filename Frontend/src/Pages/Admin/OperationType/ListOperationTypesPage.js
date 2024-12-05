import React, { useEffect, useState } from 'react';
import OperationTypesList from '../../../Components/OperationTypes/OperationTypesList';
import OperationTypeDetailsPage from './OperationTypeDetailsPage';
import fetchApi from '../../../Services/fetchApi';
import '../../../Components/OperationTypes/OperationTypeList.css';

const ListOperationTypesPage = () => {
    const [operationTypes, setOperationTypes] = useState([]);
    const [selectedOperationType, setSelectedOperationType] = useState(null);
    const [staffMembers, setStaffMembers] = useState([]);
    const [showActive, setShowActive] = useState(true);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [showDetails, setShowDetails] = useState(false);

    useEffect(() => {
        const fetchOperationTypes = async () => {
            try {
                const data = await fetchApi('/operationTypes/all');
                setOperationTypes(data);
            } catch (error) {
                console.error('Error fetching operation types:', error);
                setError('Failed to fetch operation types. Please try again later.');
            } finally {
                setLoading(false);
            }
        };

        const fetchStaffMembers = async () => {
            try {
                const data = await fetchApi('/staff/all');
                setStaffMembers(data);
            } catch (error) {
                console.error('Error fetching staff members:', error);
                setError('Failed to fetch staff members. Please try again later.');
            }
        };

        fetchOperationTypes();
        fetchStaffMembers();
    }, []);

    const handleSelect = (type) => {
        setSelectedOperationType(type);
        setShowDetails(true);
    };

    const handleBackToList = () => {
        setSelectedOperationType(null);
        setShowDetails(false);
    };

    const filteredOperationTypes = operationTypes.filter(type => type.isActive === showActive);

    if (loading) {
        return <div>Loading operation types...</div>;
    }

    if (error) {
        return <div style={{ color: 'red' }}>{error}</div>;
    }

    return (
        <div>
            <h1>List of Operation Types</h1>

            {!showDetails ? (
                <>
                    <button onClick={() => setShowActive(!showActive)}>
                        {showActive ? 'Show Inactive' : 'Show Active'}
                    </button>
                    {filteredOperationTypes.length > 0 ? (
                        <>
                            <p>Click on an operation type to see details.</p>
                            <OperationTypesList operationTypes={filteredOperationTypes} onSelect={handleSelect} />
                        </>
                    ) : (
                        <p>No operation types available to list.</p>
                    )}
                </>
            ) : (
                <>
                    <OperationTypeDetailsPage
                        operationType={selectedOperationType}
                        staffMembers={staffMembers}
                        onBack={handleBackToList}
                    />
                    <button onClick={handleBackToList}>Back to List</button>
                </>
            )}
        </div>
    );
};

export default ListOperationTypesPage;