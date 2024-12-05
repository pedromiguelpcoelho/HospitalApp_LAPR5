using DDDSample1.Domain.Shared;

public class Capacity : IValueObject
{
    public int Value { get; private set; }

    // Required for EF Core
    protected Capacity() { }

    public Capacity(int value)
    {
        if (value <= 0)
        {
            throw new BusinessRuleValidationException("Capacity must be a positive integer.");
        }

        Value = value;
    }

    public static implicit operator int(Capacity capacity) => capacity.Value;

    public static implicit operator Capacity(int capacity) => new Capacity(capacity);

    public override string ToString() => Value.ToString();
}