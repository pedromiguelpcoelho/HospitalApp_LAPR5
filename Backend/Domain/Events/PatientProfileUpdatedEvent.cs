using System;
using DDDSample1.Domain.OperationRequests;

namespace DDDSample1.Domain.Events
{
    public class PatientProfileUpdatedEvent {
        public Guid PatientId { get; private set; }
        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public PatientProfileUpdatedEvent(Guid patientId, FullName fullName, Email email)
        {
            PatientId = patientId;
            FullName = fullName;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}