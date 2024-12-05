import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, useLocation } from 'react-router-dom';
import AppRoutes from './Routes/index';
import './index.css';
import Cookies from 'js-cookie';

function App() {
    const [user, setUser] = useState(null);
    const [showLogin, setShowLogin] = useState(true);

    useEffect(() => {
        const checkUser = () => {
            const token = Cookies.get('token');
            const role = Cookies.get('role');
            const name = Cookies.get('name');
            if (token && role) {
                setUser({ token, role, name });
            }
        };
        checkUser();
    }, []);

    const logout = () => {
        Cookies.remove('token');
        Cookies.remove('role');
        Cookies.remove('name');
        setUser(null);
        alert("You are logged out!");
    };

    const toggleForm = () => {
        setShowLogin(!showLogin);
    };

    return (
        <Router>
            <div>
                <ConditionalWelcome />
                {user ? (
                    <div>
                        <AppRoutes user={user} showLogin={showLogin} toggleForm={toggleForm} setUser={setUser} logout={logout} />
                    </div>
                ) : (
                    <div className="auth-forms">
                        <AppRoutes user={user} showLogin={showLogin} toggleForm={toggleForm} setUser={setUser} />
                        <ConditionalButton showLogin={showLogin} toggleForm={toggleForm} />
                    </div>
                )}
            </div>
        </Router>
    );
}

// Novo componente para condicionar a exibição da mensagem de boas-vindas
function ConditionalWelcome() {
    const location = useLocation();

    // Exibe a mensagem de boas-vindas apenas se não estiver na rota /admin, /doctor, /patient ou /terms
    if (location.pathname.startsWith('/admin') || 
        location.pathname.startsWith('/doctor') || 
        location.pathname.startsWith('/patient') || 
        location.pathname.startsWith('/terms')) {
        return null;
    }

    return (
        <div className="welcome-container">
            <h1 className="welcome-message">Welcome to Hospitalar Software</h1>
        </div>
    );
}

// Novo componente para condicionar a exibição do botão de alternância de formulário
function ConditionalButton({ showLogin, toggleForm }) {
    const location = useLocation();

    // Exibe o botão apenas se não estiver na rota /terms
    if (location.pathname.startsWith('/terms')) {
        return null;
    }

    return (
        <button onClick={toggleForm}>
            {showLogin ? "Don't have an Account? Sign up" : "Already have an account? Sign in"}
        </button>
    );
}

export default App;