using System;
using DDDSample1.Domain.OperationRequests;

namespace DDDSample1.Domain.Events
{
    public class PatientProfileCreadtedEvent {
        public Guid PatientId { get; private set; }
        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public PatientProfileCreadtedEvent(Guid patientId, FullName fullName, Email email)
        {
            PatientId = patientId;
            FullName = fullName;
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }
    }
}