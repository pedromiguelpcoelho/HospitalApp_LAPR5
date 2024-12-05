using System;

namespace DDDSample1.Domain.Patients {
    public class PatientProfileDTO {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public long ContactInformation { get; set; }
        public Gender Gender { get; set; }
        public long EmergencyContact { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string AllergiesMedicalCond { get; set; }
        public string AppointmentHistory { get; set; }
    }
}