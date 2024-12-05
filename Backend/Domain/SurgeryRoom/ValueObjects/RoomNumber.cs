using DDDSample1.Domain.Shared;


namespace DDDSample1.Domain.SurgeryRooms
{
public class RoomNumber : IValueObject
{
    public int Value { get; private set; }

    // Required for EF Core
    protected RoomNumber() { }

    public RoomNumber(int value)
    {
        if (value <= 0)
        {
            throw new BusinessRuleValidationException("Room number must be a positive integer.");
        }

        Value = value;
    }

    public static implicit operator int(RoomNumber roomNumber) => roomNumber.Value;

    public static implicit operator RoomNumber(int roomNumber) => new RoomNumber(roomNumber);

    public override string ToString() => Value.ToString();
}
}