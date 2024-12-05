using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.SurgeryRooms
{

public class RoomType : IValueObject
{
    public string Value { get; private set; }

    // Required for EF Core
    protected RoomType() { }

    public RoomType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleValidationException("Type can't be null or empty.");
        }

        Value = value;
    }

    public static implicit operator string(RoomType type) => type.Value;

    public static implicit operator RoomType(string type) => new RoomType(type);

    public override string ToString() => Value;
}
}