import React, { useState, useEffect } from 'react';
import OperationRequestForm from '../../Components/OperationRequests/AddOperationRequestForm';
import fetchApi from '../../Services/fetchApi';
import './AddOperationRequestPage.css';

function AddOperationRequestPage() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [patients, setPatients] = useState([]);
    const [operationTypes, setOperationTypes] = useState([]);
    const [doctors, setDoctors] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [patientsResponse, operationTypesResponse, doctorsResponse] = await Promise.all([
                    fetchApi('/PatientProfile/all'),
                    fetchApi('/OperationTypes/all'),
                    fetchApi('/Staff/all')
                ]);

                setPatients(patientsResponse);
                setOperationTypes(operationTypesResponse);
                setDoctors(doctorsResponse);
            } catch (error) {
                setError(error.message);
            }
        };

        fetchData();
    }, []);

    const createOperationRequest = async (data) => {
        setLoading(true);
        setError(null);
        setSuccess(null);
    
        try {
            const response = await fetchApi('/OperationRequests/add', {
                method: 'POST',
                body: JSON.stringify(data),
            });
    
            if (response.status === 409) {
                setError(response.message);
            } else {
                setSuccess("Operation request created successfully!");
            }
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <OperationRequestForm 
                onCreateRequest={createOperationRequest} 
                patients={patients} 
                operationTypes={operationTypes} 
                doctors={doctors} 
            />
            {loading && <p>Creating...</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {success && <p style={{ color: 'green' }}>{success}</p>}
        </div>
    );
}

export default AddOperationRequestPage;