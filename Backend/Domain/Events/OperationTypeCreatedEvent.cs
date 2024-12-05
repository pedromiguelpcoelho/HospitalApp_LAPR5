using System;
using System.Collections.Generic;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Domain.Events;
public class OperationTypeCreatedEvent
{
    public Guid OperationTypeId { get; set; }
    public Name Name { get; set; }
    public List<StaffId> RequiredStaff { get; set; }
    public int EstimatedDuration { get; set; }
    public DateTime CreatedAt { get; set; }
    //public string CreatedBy { get; set; }

    public OperationTypeCreatedEvent(Guid operationTypeId, Name name, List<StaffId> requiredStaff, int estimatedDuration)
    {
        OperationTypeId = operationTypeId;
        Name = name;
        RequiredStaff = requiredStaff;
        EstimatedDuration = estimatedDuration;
        CreatedAt = DateTime.UtcNow;
    }
}
