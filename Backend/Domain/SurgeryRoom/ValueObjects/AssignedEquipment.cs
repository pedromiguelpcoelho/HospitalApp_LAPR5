using System.Collections.Generic;
using DDDSample1.Domain.Shared;

public class AssignedEquipment : IValueObject
{
    public List<string> Value { get; private set; }

    // Required for EF Core
    protected AssignedEquipment() { }

    public AssignedEquipment(List<string> value)
    {
        if (value == null || value.Count == 0)
        {
            throw new BusinessRuleValidationException("Assigned equipment list can't be null or empty.");
        }

        Value = value;
    }

    public static implicit operator List<string>(AssignedEquipment assignedEquipment) => assignedEquipment.Value;

    public static implicit operator AssignedEquipment(List<string> assignedEquipment) => new AssignedEquipment(assignedEquipment);

    public override string ToString() => string.Join(", ", Value);
} 