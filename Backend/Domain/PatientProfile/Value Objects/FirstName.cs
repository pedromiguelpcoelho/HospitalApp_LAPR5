using DDDSample1.Domain.Shared;

public class FirstName : IValueObject
{
    public string Value { get; private set; }

    // Required for EF Core
    protected FirstName() { }

    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleValidationException("First name can't be null or empty.");
        }

        Value = value;
    }

    public static implicit operator string(FirstName firstName) => firstName.Value;

    public static implicit operator FirstName(string firstName) => new FirstName(firstName);

    public override string ToString() => Value;
}