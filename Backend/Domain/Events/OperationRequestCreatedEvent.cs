using System;
using DDDSample1.Domain.OperationRequests;

namespace DDDSample1.Domain.Events
{
    public class OperationRequestCreatedEvent
    {
        public Guid OperationRequestId { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }
        public Guid OperationTypeId { get; private set; }
        public Priority Priority { get; private set; }
        public DateTime SuggestedDeadline { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public OperationRequestCreatedEvent(Guid operationRequestId, Guid patientId, Guid doctorId, Guid operationTypeId, Priority priority, DateTime suggestedDeadline)
        {
            OperationRequestId = operationRequestId;
            PatientId = patientId;
            DoctorId = doctorId;
            OperationTypeId = operationTypeId;
            Priority = priority;
            SuggestedDeadline = suggestedDeadline;
            CreatedAt = DateTime.UtcNow;
        }
    }
}