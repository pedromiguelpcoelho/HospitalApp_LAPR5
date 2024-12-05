import React, { useState } from 'react';
import './SearchOperationRequest.css';

const SearchOperationRequestForm = ({ onSearch }) => {
    const [priority, setPriority] = useState('');
    const [operationTypeName, setOperationTypeName] = useState('');
    const [patientName, setPatientName] = useState('');
    const [expectedDueDate, setExpectedDueDate] = useState('');
    const [requestDate, setRequestDate] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        onSearch({priority, operationTypeName, patientName, expectedDueDate, requestDate });
    };

    return (
        <form id="search-request-form" onSubmit={handleSubmit}>
            <h2>Search Operation Requests</h2>

            <label>Patient's Name</label>
            <input
                type="text"
                placeholder="Insert..."
                value={patientName}
                onChange={(e) => setPatientName(e.target.value)}
            />

            <label>Operation Type's Name</label>
            <input
                type="text"
                placeholder="Insert..."
                value={operationTypeName}
                onChange={(e) => setOperationTypeName(e.target.value)}
            />

            <label>Priority</label>
            <select value={priority} onChange={(e) => setPriority(e.target.value)}>
                <option value="">Select Priority</option>
                <option value="Elective">Elective</option>
                <option value="Urgent">Urgent</option>
                <option value="Emergency">Emergency</option>
            </select>

            <label>Expected Due Date</label>
            <input
                type="datetime-local"
                placeholder="Expected Due Date"
                value={expectedDueDate}
                onChange={(e) => setExpectedDueDate(e.target.value)}
            />

            <label>Request Date</label>
            <input
                type="datetime-local"
                placeholder="Request Date"
                value={requestDate}
                onChange={(e) => setRequestDate(e.target.value)}
            />

            <button type="submit">Search</button>
        </form>
    );
};

export default SearchOperationRequestForm;