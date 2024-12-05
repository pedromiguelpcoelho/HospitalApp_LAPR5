import React, { useState, useEffect } from 'react';
import UpdateOperationRequestForm from '../../Components/OperationRequests/UpdateOperationRequestForm';
import fetchApi from '../../Services/fetchApi';

const UpdateOperationRequestPage = () => {
    const [operationRequests, setOperationRequests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    useEffect(() => {
        const fetchMyRequests = async () => {
            try {
                const data = await fetchApi('/operationrequests/searchByDoctor');
                if (Array.isArray(data.operationRequests)) {
                    setOperationRequests(data.operationRequests);
                } else {
                    throw new Error('Unexpected data format.');
                }
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchMyRequests();
    }, []);

    const handleUpdateRequest = async (updatedRequest) => {
        setLoading(true);
        setError(null);
        setSuccess(null);

        try {
            await fetchApi(`/operationrequests/update/${updatedRequest.id}`, {
                method: 'PATCH',
                body: JSON.stringify(updatedRequest),
            });

            setSuccess('Operation request updated successfully!');
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h1>Update Operation Request</h1>
            {loading && <p>Loading...</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {success && <p style={{ color: 'green' }}>{success}</p>}
            {!loading && !error && (
                <UpdateOperationRequestForm
                    operationRequests={operationRequests}
                    onUpdateRequest={handleUpdateRequest}
                />
            )}
        </div>
    );
};

export default UpdateOperationRequestPage;