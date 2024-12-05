import React from 'react';
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ user, role, children }) => {
    if (!user) {
        // Se o usuário não estiver autenticado, redirecione para a página de login
        return <Navigate to="/" />;
    }

    if (role && user.role !== role) {
        // Se o usuário não tiver o papel necessário, redirecione para a página de não autorizado
        return <Navigate to="/unauthorized" />;
    }

    // Se o usuário estiver autenticado e tiver o papel necessário, renderize o componente filho
    return children;
};

export default ProtectedRoute;