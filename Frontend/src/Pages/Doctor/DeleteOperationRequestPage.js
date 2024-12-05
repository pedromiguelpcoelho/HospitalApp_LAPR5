import React, { useState, useEffect } from 'react';
import DeleteOperationRequestForm from '../../Components/OperationRequests/DeleteOperationRequestForm';
import fetchApi from '../../Services/fetchApi';

const DeleteOperationRequestPage = () => {
    const [operationRequests, setOperationRequests] = useState([]);
    const [loading, setLoading] = useState(false);
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
            }
        };

        fetchMyRequests();
    }, []);

    const handleDelete = async (operationRequestId) => {
        const isConfirmed = window.confirm("Are you sure you want to delete this operation request?");
        if (!isConfirmed) {
            return;
        }
        setLoading(true);
        setError(null);
        setSuccess(null);
        try {
            await fetchApi(`/operationrequests/delete/${operationRequestId}`, {
                method: 'DELETE',
            });

            setSuccess('Operation request deleted successfully!');
            setOperationRequests(operationRequests.filter(request => request.id !== operationRequestId));
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h1>Delete Operation Request</h1>
            {loading && <p>Loading...</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {success && <p style={{ color: 'green' }}>{success}</p>}
            <DeleteOperationRequestForm operationRequests={operationRequests} onDelete={handleDelete} />
        </div>
    );
};

export default DeleteOperationRequestPage;