using System;

namespace DDDSample1.Domain.OperationRequests
{
    public class UpdatingOperationRequestDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Priority Priority { get; set; }
        public DateTime SuggestedDeadline { get; set; }
    }
}