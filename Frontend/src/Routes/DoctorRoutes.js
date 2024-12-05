import React from 'react';
import { Route } from 'react-router-dom';
import DoctorDashboard from '../Components/Doctor/DoctorDashboard';
import AddOperationRequestPage from '../Pages/Doctor/AddOperationRequestPage';
import SearchOperationRequestPage from '../Pages/Doctor/SearchOperationRequestPage';
import DeleteOperationRequestPage from '../Pages/Doctor/DeleteOperationRequestPage';
import UpdateOperationRequestPage from '../Pages/Doctor/UpdateOperationRequestPage';
import HospitalFloor from '../Pages/Doctor/HospitalFloor';
import ProtectedRoute from './ProtectedRoute';

const DoctorRoutes = ({ user, logout }) => {
    return (
        <>
            <Route path="/doctor/*" element={<ProtectedRoute user={user} role="Doctors"><DoctorDashboard user={user} logout={logout} /></ProtectedRoute>}>
            <Route path="add-request-operation" element={<AddOperationRequestPage />} />
            <Route path="search-request-operation" element={<SearchOperationRequestPage />} />
            <Route path="delete-request-operation" element={<DeleteOperationRequestPage />} />
            <Route path="update-request-operation" element={<UpdateOperationRequestPage />} />
            <Route path="hospital-floor" element={<HospitalFloor />} /> 
            {/* Add other doctor routes here */}
            </Route>
        </>
    );
};

export default DoctorRoutes;