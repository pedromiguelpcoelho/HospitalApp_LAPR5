using System;
using System.Linq;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients {
    public class PatientProfile : Entity<PatientProfileId>, IAggregateRoot
    {
        private static readonly SequentialNumberManager _sequentialNumberManager = new SequentialNumberManager("MedicalRecordNumber.txt");
        public FirstName FirstName { get; set; }
        public LastName LastName { get; set; }
        public FullName FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Email Email { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public Gender Gender { get; set; }
        public long EmergencyContact { get; set; }
        public MedicalRecordNumber MedicalRecordNumber { get; set; }
        public string AllergiesMedicalCond { get; set; }
        public string AppointmentHistory { get; set; }
        public bool IsMarkedForDeletion { get; private set; }
        public DateTime? DeletionScheduledDate { get; private set; }

        public PatientProfile(FirstName firstName, LastName lastName, FullName fullName, DateTime dateOfBirth, Email email,
            ContactInformation contactInformation)
        {
            this.Id = new PatientProfileId(Guid.NewGuid());
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.ContactInformation = contactInformation;
            this.MedicalRecordNumber = GenerateMedicalRecordNumber();
        }

        public void UpdateDetails(FirstName firstName, LastName lastName, FullName fullName, DateTime dateOfBirth,
            Email email, ContactInformation contactInformation)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.ContactInformation = contactInformation;
        }

        private string GenerateMedicalRecordNumber() {
            var now = DateTime.Now;
            var year = now.Year.ToString();
            var month = now.Month.ToString("D2");
            var sequentialNumber = _sequentialNumberManager.GetNextSequentialNumber().ToString("D6");

            return $"{year}{month}{sequentialNumber}";
        }

        public void AppendToAppointmentHistory(string operationTypeName, DateTime suggestedDeadline) {
            var appointmentEntry = $"Operation: {operationTypeName}, Date: {suggestedDeadline.ToShortDateString()}";
            if (string.IsNullOrEmpty(this.AppointmentHistory)) {
                this.AppointmentHistory = appointmentEntry;
            }else {
                this.AppointmentHistory += $"; {appointmentEntry}";
            }
        }
        
        public void RemoveFromAppointmentHistory(string operationTypeName, DateTime suggestedDeadline) {
            var appointmentEntry = $"Operation: {operationTypeName}, Date: {suggestedDeadline.ToShortDateString()}";
            if (!string.IsNullOrEmpty(this.AppointmentHistory)) {
                var entries = this.AppointmentHistory.Split("; ").ToList();
                entries.Remove(appointmentEntry);
                this.AppointmentHistory = entries.Any() ? string.Join("; ", entries) : null;
            }
        }

        public void MarkForDeletion()
        {
            IsMarkedForDeletion = true;
            DeletionScheduledDate = DateTime.UtcNow.AddMinutes(2);
        }
    }
}