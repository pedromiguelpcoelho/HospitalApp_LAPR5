import React from 'react';
import { Route } from 'react-router-dom';
import AdminDashboard from '../Components/Admin/AdminDashboard';
import AddOperationTypesPage from '../Pages/Admin/OperationType/AddOperationTypesPage';
import GetOperationTypesPage from '../Pages/Admin/OperationType/ListOperationTypesPage';
import UpdateOperationTypesPage from '../Pages/Admin/OperationType/UpdateOperationTypesPage';
import DeactivateOperationTypesPage from '../Pages/Admin/OperationType/DeactivateOperationTypesPage';
import OperationTypeDetailsPage from '../Pages/Admin/OperationType/OperationTypeDetailsPage';
import AddPatientProfilePage from '../Pages/Admin/PatientProfile/AddPatientProfilePage';
import UpdatePatientProfilePage from '../Pages/Admin/PatientProfile/UpdatePatientProfilePage';
import DeletePatientProfilePage from '../Pages/Admin/PatientProfile/DeletePatientProfilePage';
import ListPatientProfilePage from '../Pages/Admin/PatientProfile/ListPatientProfilePage';
import AddStaffPage from '../Pages/Admin/StaffProfile/AddStafePage';
import ListStaffPage from '../Pages/Admin/StaffProfile/ListStaffPage';
import UpdateStaffPage from '../Pages/Admin/StaffProfile/UpdateStaffPage';
import DeleteStaffPage from '../Pages/Admin/StaffProfile/DeleteStaffPage';
import SearchOperationRequestsAdminPage from '../Pages/Admin/OperationRequest/SearchOperationRequestsAdminPage';
import ProtectedRoute from './ProtectedRoute';

const AdminRoutes = ({ user, logout }) => (
    <>
        <Route path="/admin/*" element={<ProtectedRoute user={user} role="Admins"><AdminDashboard user={user} logout={logout} /></ProtectedRoute>}>
            <Route path="addoperationtype" element={<AddOperationTypesPage />} />
            <Route path="updateoperationtype" element={<UpdateOperationTypesPage />} />
            <Route path="updateoperationtype/:id" element={<UpdateOperationTypesPage />} />
            <Route path="deactivateoperationtype" element={<DeactivateOperationTypesPage />} />
            <Route path="deactivateoperationtype/:id" element={<DeactivateOperationTypesPage />} />
            <Route path="getoperationtypes" element={<GetOperationTypesPage />} />
            <Route path="operationtype/:id" element={<OperationTypeDetailsPage />} />
            
            <Route path="addpatientprofile" element={<AddPatientProfilePage />} />
            <Route path="updatepatientprofile" element={<UpdatePatientProfilePage />} />
            <Route path="deletepatientprofile" element={<DeletePatientProfilePage />} />
            <Route path="listpatientprofile" element={<ListPatientProfilePage />} />

            <Route path="addstaff" element={<AddStaffPage />} />     
            <Route path="updatestaff" element={<UpdateStaffPage />} />       
            <Route path="deactivatestaff" element={<DeleteStaffPage />} />            
            <Route path="liststaff" element={<ListStaffPage />} />

            <Route path="search-operation-request-admin" element={<SearchOperationRequestsAdminPage />} />
        </Route>
    </>
);

export default AdminRoutes;