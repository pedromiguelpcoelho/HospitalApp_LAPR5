import React from 'react';

const AppointmentsList = ({ appointments }) => {
    return (
        <ul>
            {appointments.map((appointment) => (
                <li key={appointment.id}>
                    <h3>Appointment ID: {appointment.id}</h3>
                    <p><strong>Date:</strong> {new Date(appointment.dateTime).toLocaleString()}</p>
                    <p><strong>Duration:</strong> {appointment.duration} minutes</p>
                    <p><strong>Status:</strong> {appointment.status}</p>
                    <p><strong>Room:</strong> {appointment.roomId}</p>
                    <p><strong>Assigned Staff:</strong> {appointment.assignedStaff.join(', ')}</p>
                    <p><strong>Operation Request:</strong> {appointment.requestId || 'No request linked'}</p>
                </li>
            ))}
        </ul>
    );
};

export default AppointmentsList;
