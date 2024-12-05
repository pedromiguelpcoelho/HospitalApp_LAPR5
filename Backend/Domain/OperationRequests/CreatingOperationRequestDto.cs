using System;

namespace DDDSample1.Domain.OperationRequests
{
    public class CreatingOperationRequestDto(
        Guid patientId,
        Guid doctorId,
        Guid operationTypeId,
        Priority priority,
        DateTime suggestedDeadline)
    {
        public Guid PatientId { get; set; } = patientId;
        public Guid DoctorId { get; set; } = doctorId;
        public Guid OperationTypeId { get; set; } = operationTypeId;
        public Priority Priority { get; set; } = priority;
        public DateTime SuggestedDeadline { get; set; } = suggestedDeadline;
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    }
}