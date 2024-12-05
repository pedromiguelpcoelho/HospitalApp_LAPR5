import React from 'react';
import { Routes } from 'react-router-dom';
import AuthRoutes from './AuthRoutes';
import AdminRoutes from './AdminRoutes';
import DoctorRoutes from './DoctorRoutes';
import PatientRoutes from './PatientRoutes';

const AppRoutes = ({ user, showLogin, toggleForm, setUser, logout }) => {
    return (
        <Routes>
            {AuthRoutes({ showLogin, setUser })}
            {AdminRoutes({ user, logout })}
            {DoctorRoutes({ user, logout })}
            {PatientRoutes({ user, logout })}
        </Routes>
    );
};

export default AppRoutes;