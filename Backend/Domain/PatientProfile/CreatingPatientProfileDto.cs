using System;

namespace DDDSample1.Domain.Patients {
    public class CreatingPatientProfileDto {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public long ContactInformation { get; set; }
        public Gender Gender { get; set; }
        public long EmergencyContact { get; set; }
        public string AllergiesMedicalCond { get; set; }
        public string AppointmentHistory { get; set; }

        public CreatingPatientProfileDto(string firstName, string lastName, string fullName, string email, DateTime dateOfBirth,
            long contactInformation, Gender gender, long emergencyContact, string allergiesMedicalCond,
            string appointmentHistory) {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.ContactInformation = contactInformation;
            this.Gender = gender;
            this.EmergencyContact = emergencyContact;
            this.AllergiesMedicalCond = allergiesMedicalCond;
            this.AppointmentHistory = appointmentHistory;
        }
        
        
        public CreatingPatientProfileDto() {
            // Parameterless constructor for JSON deserialization
        }
        
        public CreatingPatientProfileDto(string firstName, string lastName, string fullName, string email, DateTime dateOfBirth,
            long contactInformation) {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.ContactInformation = contactInformation;
        }
        
    }
}