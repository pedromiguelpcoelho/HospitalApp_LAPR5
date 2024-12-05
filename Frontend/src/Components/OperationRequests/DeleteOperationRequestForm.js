import React, { useState } from 'react';
import './DeleteOperationRequestForm.css';

const DeleteOperationRequestForm = ({ operationRequests, onDelete }) => {
    const [selectedRequestId, setSelectedRequestId] = useState('');
    const [selectedRequestDetails, setSelectedRequestDetails] = useState(null);

    const handleSelectChange = (e) => {
        const requestId = e.target.value;
        setSelectedRequestId(requestId);
        const selectedRequest = operationRequests.find(request => request.id === requestId);
        setSelectedRequestDetails(selectedRequest);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onDelete(selectedRequestId);
    };

    return (
        <form onSubmit={handleSubmit} className="delete-request-form-container">
            <div className="delete-request-form-field">
                <label className="delete-request-form-label">
                    Select Operation Request ID:
                </label>
                <select
                    value={selectedRequestId}
                    onChange={handleSelectChange}
                    required
                    className="delete-request-form-select"
                >
                    <option value="">Select an ID</option>
                    {operationRequests.map((request) => (
                        <option key={request.id} value={request.id}>
                            {request.id + ' - ' + request.patientName}
                        </option>
                    ))}
                </select>
            </div>
            {selectedRequestDetails && (
                <div className="delete-request-form-details">
                    <h3>Operation Request Details</h3>
                    <p><strong>ID:</strong> {selectedRequestDetails.id}</p>
                    <p><strong>Patient Name:</strong> {selectedRequestDetails.patientName}</p>
                    <p><strong>Operation Type:</strong> {selectedRequestDetails.operationTypeName}</p>
                    <p><strong>Doctor Name:</strong> {selectedRequestDetails.doctorName}</p>
                    <p><strong>Priority:</strong> {selectedRequestDetails.priority}</p>
                    <p><strong>Expected Due Date:</strong> {new Date(selectedRequestDetails.suggestedDeadline).toLocaleString()}</p>
                    <p><strong>Request Date:</strong> {new Date(selectedRequestDetails.requestDate).toLocaleString()}</p>
                </div>
            )}
            <button type="submit" className="delete-request-form-button">
                Delete
            </button>
        </form>
    );
};

export default DeleteOperationRequestForm;