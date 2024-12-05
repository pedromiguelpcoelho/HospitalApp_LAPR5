import React from 'react';
import { Route } from 'react-router-dom';
import LoginPage from '../Pages/Auth/LoginPage';
import SignUpPage from '../Pages/Auth/SignUpPage';
import TermsAndConditions from '../Pages/Auth/TermsAndConditionsPage';

const AuthRoutes = ({ showLogin, setUser }) => (
    <>
        <Route path="/" element={showLogin ? <LoginPage setUser={setUser} /> : <SignUpPage />} />
        <Route path="/terms" element={<TermsAndConditions />} />    </>
);

export default AuthRoutes;