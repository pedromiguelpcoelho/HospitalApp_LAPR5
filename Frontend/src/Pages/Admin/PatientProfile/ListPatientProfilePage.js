import React, { useState, useEffect } from 'react';
import PatientProfileList from '../../../Components/PatientProfile/PatientProfileList';
import fetchApi from '../../../Services/fetchApi';
import './ListPatientProfilePage.css';

function ListPatientProfilePage() {
    const [profiles, setProfiles] = useState([]);
    const [filteredProfiles, setFilteredProfiles] = useState([]);
    const [selectedProfile, setSelectedProfile] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [searchParams, setSearchParams] = useState({
        fullName: '',
        medicalRecordNumber: '',
        dateOfBirth: '',
        email: '',
        phoneNumber: ''
    });
    const [selectedFilter, setSelectedFilter] = useState('');

    const fetchPatientProfiles = async () => {
        setLoading(true);
        setError(null);

        try {
            const queryParams = new URLSearchParams({ [selectedFilter]: searchParams[selectedFilter] }).toString();
            const data = await fetchApi(`/PatientProfile/all?${queryParams}`, { method: 'GET' });
            setProfiles(data);
            setFilteredProfiles(data);

            if (data.length === 0) {
                throw new Error('No patient profiles found.');
            }
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    const handleProfileClick = (profile) => {
        setSelectedProfile(profile);
    };

    const handleSearchChange = (e) => {
        const { name, value } = e.target;
        setSearchParams(prevParams => ({
            ...prevParams,
            [name]: value
        }));
    };

    const handleFilterChange = (e) => {
        setSelectedFilter(e.target.value);
        setSearchParams({
            fullName: '',
            medicalRecordNumber: '',
            dateOfBirth: '',
            email: '',
            phoneNumber: ''
        });
    };

    useEffect(() => {
        const filterProfiles = () => {
            const filtered = profiles.filter(profile => {
                return (
                    (searchParams.fullName === '' || profile.fullName.toLowerCase().includes(searchParams.fullName.toLowerCase())) &&
                    (searchParams.medicalRecordNumber === '' || profile.medicalRecordNumber.toLowerCase().includes(searchParams.medicalRecordNumber.toLowerCase())) &&
                    (searchParams.dateOfBirth === '' || profile.dateOfBirth.includes(searchParams.dateOfBirth)) &&
                    (searchParams.email === '' || profile.email.toLowerCase().includes(searchParams.email.toLowerCase())) &&
                    (searchParams.phoneNumber === '' || profile.phoneNumber.toLowerCase().includes(searchParams.phoneNumber.toLowerCase()))
                );
            });
            setFilteredProfiles(filtered);
        };

        filterProfiles();
    }, [searchParams, profiles]);

    return (
        <div className="list-patient-profile-container">
            <h1>Patient Profiles</h1>
            {error && <p className="error-message">{error}</p>}

            <PatientProfileList
                searchParams={searchParams}
                selectedFilter={selectedFilter}
                onSearchChange={handleSearchChange}
                onFilterChange={handleFilterChange}
            />

            <button
                className="search-button"
                onClick={fetchPatientProfiles}
                disabled={loading || !selectedFilter || !searchParams[selectedFilter]}
            >
                {loading ? 'Searching...' : 'Search'}
            </button>

            {loading ? (
                <p>Loading...</p>
            ) : (
                <div className="patient-profiles-list">
                    {filteredProfiles.map(profile => (
                        <div key={profile.id} className="patient-profile-card" onClick={() => handleProfileClick(profile)}>
                            <p>{profile.fullName}</p>
                        </div>
                    ))}
                </div>
            )}

            {selectedProfile && (
                <div className="selected-profile-details">
                    <h2>Patient Details</h2>
                    <p><strong>First Name:</strong> {selectedProfile.firstName}</p>
                    <p><strong>Last Name:</strong> {selectedProfile.lastName}</p>
                    <p><strong>Full Name:</strong> {selectedProfile.fullName}</p>
                    <p><strong>Email:</strong> {selectedProfile.email}</p>
                    <p><strong>Date of Birth:</strong> {new Date(selectedProfile.dateOfBirth).toLocaleDateString()}</p>
                    <p><strong>Contact Information:</strong> {selectedProfile.contactInformation}</p>
                    <p><strong>Medical Record Number:</strong> {selectedProfile.medicalRecordNumber}</p>
                </div>
            )}
        </div>
    );
}

export default ListPatientProfilePage;
