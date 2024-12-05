using DDDSample1.Domain.Shared;

public class Name : IValueObject
{
    public string Value { get; private set; }

    // Required for EF Core
    protected Name() { }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleValidationException("Name can't be null or empty.");
        }

        Value = value;
    }

    public static implicit operator string(Name name) => name.Value;

    public static implicit operator Name(string name) => new Name(name);

    public override string ToString() => Value;
}
