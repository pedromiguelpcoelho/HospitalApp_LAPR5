using System;

namespace DDDSample1.Domain.Events
{
    public class PatientProfileDeletedEvent
    {
        public Guid PatientId { get; private set; }
        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        public DateTime DeletedAt { get; private set; }

        public PatientProfileDeletedEvent(Guid patientId, FullName fullName, Email email)
        {
            PatientId = patientId;
            FullName = fullName;
            Email = email;
            DeletedAt = DateTime.UtcNow;
        }
    }
}