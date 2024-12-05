using DDDSample1.Domain.Shared;

public class FullName : IValueObject
{
    public string Value { get; private set; }

    // Required for EF Core
    protected FullName() { }

    public FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleValidationException("First name can't be null or empty.");
        }

        Value = value;
    }

    public static implicit operator string(FullName fullName) => fullName.Value;

    public static implicit operator FullName(string fullName) => new FullName(fullName);

    public override string ToString() => Value;
}