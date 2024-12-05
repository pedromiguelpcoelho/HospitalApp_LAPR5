using DDDSample1.Domain.Shared;

public class LastName : IValueObject
{
    public string Value { get; private set; }

    // Required for EF Core
    protected LastName() { }

    public LastName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleValidationException("Last name can't be null or empty.");
        }

        Value = value;
    }

    public static implicit operator string(LastName lastName) => lastName.Value;

    public static implicit operator LastName(string lastName) => new LastName(lastName);

    public override string ToString() => Value;
}