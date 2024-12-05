import React, { useState } from 'react';
import './AddOperationRequestForm.css';

function OperationRequestForm({ onCreateRequest, patients, operationTypes, doctors }) {
    const [patientId, setPatientId] = useState('');
    const [operationTypeId, setOperationTypeId] = useState('');
    const [doctorId, setDoctorId] = useState('');
    const [priority, setPriority] = useState('Elective');
    const [suggestedDeadline, setSuggestedDeadline] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = () => {
        setError('');

        if (!patientId || !operationTypeId || !doctorId || !suggestedDeadline) {
            setError('All fields are required.');
            return;
        }

        const data = {
            patientId,
            operationTypeId,
            doctorId,
            priority,
            suggestedDeadline: new Date(suggestedDeadline)
        };

        onCreateRequest(data);

        setPatientId('');
        setOperationTypeId('');
        setDoctorId('');
        setPriority('Elective');
        setSuggestedDeadline('');
    };

    return (
        <div id="add-request-form">
            <h1>Add Operation Request</h1>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            
            <select value={patientId} onChange={(e) => setPatientId(e.target.value)} required>
                <option value="">Select Patient</option>
                {patients.map(patient => (
                    <option key={patient.id} value={patient.id}>{patient.fullName}</option>
                ))}
            </select>
            
            <select value={operationTypeId} onChange={(e) => setOperationTypeId(e.target.value)} required>
                <option value="">Select Operation Type</option>
                {operationTypes.map(type => (
                    <option key={type.id} value={type.id}>{type.name}</option>
                ))}
            </select>
            
            <select value={doctorId} onChange={(e) => setDoctorId(e.target.value)} required>
                <option value="">Select Doctor</option>
                {doctors.map(doctor => (
                    <option key={doctor.id} value={doctor.id}>{doctor.name}</option>
                ))}
            </select>
            
            <select value={priority} onChange={(e) => setPriority(e.target.value)} required>
                <option value="Elective">Elective</option>
                <option value="Urgent">Urgent</option>
                <option value="Emergency">Emergency</option>
            </select>
            
            <input
                type="datetime-local"
                value={suggestedDeadline}
                onChange={(e) => setSuggestedDeadline(e.target.value)}
                required
            />
            
            <button onClick={handleSubmit}>Create</button>
            
        </div>
    );
}

export default OperationRequestForm;