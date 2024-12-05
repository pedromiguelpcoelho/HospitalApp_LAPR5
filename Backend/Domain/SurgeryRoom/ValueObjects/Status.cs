using DDDSample1.Domain.Shared;
using System;

public class Status : IValueObject
{
    public string Value { get; private set; }

    private static readonly string[] AllowedValues = { "Available", "Occupied", "Under maintenance" };

    // Required for EF Core
    protected Status() { }

    public Status(string value)
    {
        if (Array.IndexOf(AllowedValues, value) < 0)
        {
            throw new BusinessRuleValidationException("Status must be one of the following values: Available, Occupied, Under maintenance.");
        }

        Value = value;
    }

    public static implicit operator string(Status status) => status.Value;

    public static implicit operator Status(string status) => new Status(status);

    public override string ToString() => Value;
}