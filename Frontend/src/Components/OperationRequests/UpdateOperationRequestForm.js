import React, { useState, useEffect } from 'react';
import './UpdateOperationRequest.css';

const UpdateOperationRequestForm = ({ operationRequests = [], onUpdateRequest }) => {
    const [selectedRequestId, setSelectedRequestId] = useState('');
    const [priority, setPriority] = useState('');
    const [suggestedDeadline, setSuggestedDeadline] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        if (selectedRequestId) {
            const selectedRequest = operationRequests.find(request => request.id === selectedRequestId);
            if (selectedRequest) {
                setPriority(selectedRequest.priority || '');
                setSuggestedDeadline(selectedRequest.suggestedDeadline || '');
            }
        }
    }, [selectedRequestId, operationRequests]);

    const handleSelectChange = (e) => {
        setSelectedRequestId(e.target.value);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        setError('');

        const updatedRequest = {
            id: selectedRequestId,
            priority: priority || undefined,
            suggestedDeadline: suggestedDeadline ? new Date(suggestedDeadline) : undefined
        };

        onUpdateRequest(updatedRequest);
    };

    return (
        <form className="update-request-form" onSubmit={handleSubmit}>
            {error && <p>{error}</p>}
            <div className="update-request-form-field">
                <label className="update-request-form-label">
                    Select Operation Request ID:
                </label>
                <select
                    value={selectedRequestId}
                    onChange={handleSelectChange}
                    required
                    className="update-request-form-select"
                >
                    <option value="">Select an ID</option>
                    {operationRequests.map((request) => (
                        <option key={request.id} value={request.id}>
                            {request.id + ' - ' + request.patientName}
                        </option>
                    ))}
                </select>
            </div>
            {selectedRequestId && (
                <>
                    <div className="update-request-form-field">
                        <label className="update-request-form-label">Priority:</label>
                        <select value={priority} onChange={(e) => setPriority(e.target.value)}>
                            <option value="">Select Priority</option>
                            <option value="Elective">Elective</option>
                            <option value="Urgent">Urgent</option>
                            <option value="Emergency">Emergency</option>
                        </select>
                    </div>
                    <div className="update-request-form-field">
                        <label className="update-request-form-label">Suggested Deadline:</label>
                        <input
                            type="datetime-local"
                            value={suggestedDeadline}
                            onChange={(e) => setSuggestedDeadline(e.target.value)}
                        />
                    </div>
                </>
            )}
            <button type="submit">Update</button>
        </form>
    );
};

export default UpdateOperationRequestForm;