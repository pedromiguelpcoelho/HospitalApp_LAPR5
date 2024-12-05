import React, { useState, useEffect } from 'react';
import SearchOperationRequestsAdminForm from '../../../Components/OperationRequests/SearchOperationRequestsAdminForm';
import fetchApi from '../../../Services/fetchApi';
import Modal from '../../../Components/Modal/Modal';
import '../../../Components/OperationRequests/SearchOperationRequest.css';

const SearchOperationRequestsAdminPage = () => {
    const [results, setResults] = useState([]);
    const [error, setError] = useState(null);
    const [expandedRequestId, setExpandedRequestId] = useState(null);

    useEffect(() => {
        const fetchAllRequests = async () => {
            try {
                const data = await fetchApi('/operationrequests/all');
                if (Array.isArray(data.operationRequests)) {
                    setResults(data.operationRequests);
                } else {
                    throw new Error('Unexpected data format.');
                }
            } catch (error) {
                setError(error.message);
            }
        };

        fetchAllRequests();
    }, []);

    const handleSearch = async (filters) => {
        try {
            const query = new URLSearchParams(filters).toString();
            const data = await fetchApi(`/operationrequests/search?${query}`);

            if (Array.isArray(data.operationRequests) && data.operationRequests.length > 0) {
                setResults(data.operationRequests);
                setError(null);
            } else {
                setResults([]);
                setError("There's no Operation Request that match those search criteria.");
            }
        } catch (error) {
            setError(error.message);
        }
    };

    const toggleDetails = (id) => {
        setExpandedRequestId(expandedRequestId === id ? null : id);
    };

    const closeModal = () => {
        setError(null);
    };

    return (
        <div className="search-page-container">
            <div className="search-form-container">
                <SearchOperationRequestsAdminForm onSearch={handleSearch} />
            </div>
            <div className="search-results-container">
                {error && <Modal message={error} onClose={closeModal} />}
                {results.length > 0 ? (
                    <ul className="search-results-list">
                        {results.map((request) => (
                            <li key={request.id} className="search-results-item">
                                <button className="search-toggle-details-button" onClick={() => toggleDetails(request.id)}>
                                    <strong>Patient: {request.patientName}</strong>
                                </button>
                                {expandedRequestId === request.id && (
                                    <div>
                                        <p><strong>ID:</strong> {request.id}</p>
                                        <p><strong>Patient:</strong> {request.patientName}</p>
                                        <p><strong>Operation Type:</strong> {request.operationTypeName}</p>
                                        <p><strong>Doctor:</strong> {request.doctorName}</p>
                                        <p><strong>Priority:</strong> {request.priority}</p>
                                        <p><strong>Expected Due Date:</strong> {new Date(request.suggestedDeadline).toLocaleString()}</p>
                                        <p><strong>Request Date:</strong> {new Date(request.requestDate).toLocaleString()}</p>
                                    </div>
                                )}
                            </li>
                        ))}
                    </ul>
                ) : (
                    <p><strong>No operation requests found.</strong></p>
                )}
            </div>
        </div>
    );
};

export default SearchOperationRequestsAdminPage;