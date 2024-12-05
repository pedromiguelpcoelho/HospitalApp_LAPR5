using System;

namespace DDDSample1.Domain.Events
{
    public class OperationRequestDeletedEvent
    {
        public Guid OperationRequestId { get; private set; }
        public DateTime DeletedAt { get; private set; }

        public OperationRequestDeletedEvent(Guid operationRequestId)
        {
            OperationRequestId = operationRequestId;
            DeletedAt = DateTime.UtcNow;
        }
    }
}