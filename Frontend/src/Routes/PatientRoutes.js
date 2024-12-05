import React from 'react';
import { Route } from 'react-router-dom';
import PatientDashboard from '../Components/Patient/PatientDashboard';
import RequestDeletionOfPatientProfilePage from '../Pages/Patient/RequestDeletionOfPatientProfilePage';
import ConfirmDeletionOfPatientProfilePage from '../Pages/Patient/ConfirmDeletionOfPatientProfilePage'; 
import UpdateMyOwnPatientProfilePage from '../Pages/Patient/UpdateMyOwnPatientProfilePage';
import MyAppointmentsPage from '../Pages/Patient/MyAppointmentsPage';
import ProtectedRoute from './ProtectedRoute';

const PatientRoutes = ({ user, logout }) => {
    return (
        <>
            <Route path="/patient/*" element={<ProtectedRoute user={user} role="Patients"><PatientDashboard user={user} logout={logout} /></ProtectedRoute>}>
                <Route path="request-deletion-patient-profile" element={<RequestDeletionOfPatientProfilePage user={user} />} />
                <Route path="confirm-deletion-patient-profile" element={<ConfirmDeletionOfPatientProfilePage user={user} />} /> 
                <Route path="update-patient-profile" element={<UpdateMyOwnPatientProfilePage user={user} />} />
                <Route path="my-appointments" element={<MyAppointmentsPage user={user} />} />
                {/* Add other patient routes here */}
            </Route>
        </>
    );
};

export default PatientRoutes;