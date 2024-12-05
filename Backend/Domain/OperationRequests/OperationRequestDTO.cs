using System;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequestDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public Guid OperationTypeId { get; set; }
        public string OperationTypeName { get; set; }
        public Priority Priority { get; set; }
        public DateTime SuggestedDeadline { get; set; }
        public DateTime RequestDate { get; set; }
    }
}