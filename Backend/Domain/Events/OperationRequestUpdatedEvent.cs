using System;
using DDDSample1.Domain.OperationRequests;

namespace DDDSample1.Domain.Events
{
    public class OperationRequestUpdatedEvent
    {
        public Guid OperationRequestId { get; private set; }
        public Priority OriginalPriority { get; private set; }
        public Priority UpdatedPriority { get; private set; }
        public DateTime OriginalDeadline { get; private set; }
        public DateTime UpdatedDeadline { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public OperationRequestUpdatedEvent(Guid operationRequestId, Priority originalPriority, Priority updatedPriority, DateTime originalDeadline, DateTime updatedDeadline)
        {
            OperationRequestId = operationRequestId;
            OriginalPriority = originalPriority;
            UpdatedPriority = updatedPriority;
            OriginalDeadline = originalDeadline;
            UpdatedDeadline = updatedDeadline;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}