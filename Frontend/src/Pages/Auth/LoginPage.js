import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../../Components/Auth/LoginForm';
import Cookies from 'js-cookie';
import fetchApi from '../../Services/fetchApi';

const LoginPage = ({ setUser }) => {
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleLogin = async (email, password) => {
        try {
            const data = await fetchApi('/auth/login-username', {
                method: 'POST',
                body: JSON.stringify({ email, password }),
            });

            Cookies.set('token', data.token, { expires: 7 });
            Cookies.set('role', data.role, { expires: 7 });
            Cookies.set('name', data.name, { expires: 7 });

            setUser({ email, role: data.role, name: data.name });

            // Redireciona para a route apropriada com base no user role obtido do token
            if (data.role === 'Admins') {
                navigate('/admin');
            } else if (data.role === 'Doctors') {
                navigate('/doctor/dashboard');
            } else if (data.role === 'Patients') {
                navigate('/patient/dashboard');
            }
        } catch (error) {
            setError('Authentication failed');
        }
    };

    return (
        <div>
            <LoginForm onLogin={handleLogin} />
            {error && <p style={{ color: 'red' }}>{error}</p>}
        </div>
    );
};

export default LoginPage;