import React, { useState } from 'react';
import AppointmentsList from '../../Components/PatientProfile/AppointmentsList';
import fetchApi from '../../Services/fetchApi';
import './MyAppointmentsPage.css';


const MyAppointmentsPage = () => {
    const [appointments, setAppointments] = useState([]);
    const [message, setMessage] = useState('');

    const fetchAppointments = async () => {
        try {
            // Fetch appointments for the authenticated patient
            const response = await fetchApi('/appointments/my-appointments', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.length === 0) {
                setMessage('No appointments found.');
            } else {
                // Enrich appointments with operation type (from Operation Request)
                const enrichedAppointments = await Promise.all(
                    response.map(async (appointment) => {
                        if (appointment.requestId) {
                            // Fetch the operation type for the requestId
                            const requestDetails = await fetchApi(`/operation-requests/${appointment.requestId}`, {
                                method: 'GET',
                                headers: {
                                    'Content-Type': 'application/json'
                                }
                            });

                            return { 
                                ...appointment, 
                                operationType: requestDetails.operationType 
                            };
                        }
                        return appointment;
                    })
                );

                setAppointments(enrichedAppointments);
                setMessage('');
            }
        } catch (error) {
            console.error('Request failed:', error);
            setMessage('Failed to load appointments.');
        }
    };

    return (
        <div>
            <button onClick={fetchAppointments}>Load My Appointments</button>
            {message && <p>{message}</p>}
            {appointments.length > 0 && <AppointmentsList appointments={appointments} />}
        </div>
    );
};

export default MyAppointmentsPage;
