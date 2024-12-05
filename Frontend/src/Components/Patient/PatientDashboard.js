import React from 'react';
import { useNavigate, Link, Outlet } from 'react-router-dom';
import './PatientDashboard.css';

const PatientDashboard = ({ user, logout }) => {
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/');
    };

    return (
        <div className="patient-dashboard-container">
            <aside className="patient-sidebar">
                <h2>PATIENT MENU</h2>
                <nav className="patient-sidebar-nav">
                    <ul>
                        <li><Link to="request-deletion-patient-profile" className="patient-nav-item">Request Account Deletion</Link></li>
                        <li><Link to="confirm-deletion-patient-profile" className="patient-nav-item">Confirm Account Deletion</Link></li> 
                        <li><Link to="update-patient-profile" className="patient-nav-item">Update My Profile</Link></li>
                        <li><Link to="my-appointments" className="patient-nav-item">See My Appointments</Link></li>                     
                        {/* Add other navigation items here */}
                    </ul>
                </nav>
                <button className="patient-logout-button" onClick={handleLogout}>Logout</button>
            </aside>
            <main className="patient-main-content">
                <Outlet />
            </main>
        </div>
    );
};

export default PatientDashboard;