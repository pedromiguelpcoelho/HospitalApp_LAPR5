import React, { useState, useEffect } from 'react';
import StaffList from '../../../Components/StaffProfile/StaffList';
import UpdateStaffForm from '../../../Components/StaffProfile/UpdateStaffForm';
import fetchApi from '../../../Services/fetchApi';
import './styles/UpdateStaffPage.css';

function UpdateStaffPage() {
   const [profiles, setProfiles] = useState([]);
   const [filteredProfiles, setFilteredProfiles] = useState([]);
   const [selectedProfile, setSelectedProfile] = useState(null);
   const [loading, setLoading] = useState(false);
   const [error, setError] = useState(null);
   const [searchParams, setSearchParams] = useState({
       name: '',
       specialization: '',
       email: '',
       phoneNumber: '',
       licenseNumber: '',
       isActive: true
   });
   const [selectedFilter, setSelectedFilter] = useState('');
   const [formData, setFormData] = useState({
       firstName: '',
       lastName: '',
       name: '',
       email: '',
       phoneNumber: '',
       specialization: '',
       role: ''
   });
   const [isEditing, setIsEditing] = useState(false); 

   useEffect(() => {
       const fetchStaffProfiles = async () => {
           setLoading(true);
           setError(null);

           try {
               const data = await fetchApi('/staff/search', { method: 'GET' });
               setProfiles(data);
               setFilteredProfiles(data);
           } catch (error) {
               setError(error.message);
           } finally {
               setLoading(false);
           }
       };

       fetchStaffProfiles();
   }, []);

   const handleProfileClick = (profile) => {
       setSelectedProfile(profile);
       setFormData({
           firstName: profile.firstName,
           lastName: profile.lastName,
           name: profile.name,
           email: profile.email,
           phoneNumber: profile.phoneNumber,
           specialization: profile.specialization,
           role: profile.role
       });
       setIsEditing(false);
   };

   const handleSearchChange = (e) => {
       const { name, value, type, checked } = e.target;
       setSearchParams(prevParams => ({
           ...prevParams,
           [name]: type === 'checkbox' ? checked : value
       }));

       setSelectedProfile(null);
   };

   const handleFilterChange = (e) => {
       setSelectedFilter(e.target.value);
       setSearchParams({
           name: '',
           specialization: '',
           email: '',
           phoneNumber: '',
           licenseNumber: '',
           isActive: true
       });

       setSelectedProfile(null);
   };

   useEffect(() => {
       const filterProfiles = () => {
           const filtered = profiles.filter(profile => {
               return (
                   (searchParams.name === '' || profile.name.toLowerCase().includes(searchParams.name.toLowerCase())) &&
                   (searchParams.specialization === '' || profile.specialization.toLowerCase().includes(searchParams.specialization.toLowerCase())) &&
                   (searchParams.email === '' || profile.email.toLowerCase().includes(searchParams.email.toLowerCase())) &&
                   (searchParams.phoneNumber === '' || profile.phoneNumber.toLowerCase().includes(searchParams.phoneNumber.toLowerCase())) &&
                   (searchParams.licenseNumber === '' || profile.licenseNumber.toLowerCase().includes(searchParams.licenseNumber.toLowerCase())) &&
                   (searchParams.isActive === null || profile.isActive === searchParams.isActive)
               );
           });
           setFilteredProfiles(filtered);
       };

       filterProfiles();
   }, [searchParams, profiles]);

   const handleUpdate = async () => {
       setLoading(true);
       setError(null);

       try {
           await fetchApi(`/staff/${selectedProfile.id}`, {
               method: 'PUT',
               body: JSON.stringify(formData)
           });

           setSelectedProfile(null);
           setFormData({
               firstName: '',
               lastName: '',
               name: '',
               email: '',
               phoneNumber: '',
               specialization: '',
               role: ''
           });
           setIsEditing(false);
           alert("Staff profile updated successfully!");
       } catch (error) {
           setError(error.message);
       } finally {
           setLoading(false);
       }
   };

   const handleCancel = () => {
       setIsEditing(false);
       setSelectedProfile(null);
       setFormData({
           firstName: '',
           lastName: '',
           name: '',
           email: '',
           phoneNumber: '',
           specialization: '',
           role: ''
       });
   };

   return (
       <div className="list-staff-container">
           <h1>Staff List</h1>
           {error && <p className="error-message">{error}</p>}

           {!isEditing && (
               <StaffList
                   searchParams={searchParams}
                   selectedFilter={selectedFilter}
                   onSearchChange={handleSearchChange}
                   onFilterChange={handleFilterChange}
               />
           )}

           {!isEditing && loading ? (
               <p>Loading...</p>
           ) : (
               !isEditing && (
                   <div className="staff-list">
                       {filteredProfiles.map(profile => (
                           <div key={profile.id} className="staff-card" onClick={() => handleProfileClick(profile)}>
                               <p>{profile.name}</p>
                           </div>
                       ))}
                   </div>
               )
           )}

           {selectedProfile && !isEditing && (
               <div className="selected-details">
                   <h2>Staff Details</h2>
                   <p><strong>Name:</strong> {selectedProfile.name}</p>
                   <p><strong>Specialization:</strong> {selectedProfile.specialization}</p>
                   <p><strong>Email:</strong> {selectedProfile.email}</p>
                   <p><strong>Phone Number:</strong> {selectedProfile.phoneNumber}</p>
                   <p><strong>License Number:</strong> {selectedProfile.licenseNumber}</p>
                   <p><strong>Active Status:</strong> {selectedProfile.isActive ? 'Active' : 'Inactive'}</p>
                   <button onClick={() => setIsEditing(true)} disabled={loading}>Update Profile</button>
               </div>
           )}

           {selectedProfile && isEditing && (
               <UpdateStaffForm
                   formData={formData}
                   onFormChange={(e) => {
                       const { name, value } = e.target;
                       setFormData(prevData => ({
                           ...prevData,
                           [name]: value
                       }));
                   }}
                   onUpdate={handleUpdate}
                   loading={loading}
               />
           )}

           {isEditing && (
               <button className="cancel-button" onClick={handleCancel}>
                   Cancel
               </button>
           )}
       </div>
   );

}

export default UpdateStaffPage;