using System;
using DDDSample1.Domain.Shared;

public class Email : IValueObject
{
    public string Value { get; private set; }

    protected Email()
    {
    }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be null or empty.");
        }

        if (!value.Contains("@") || !value.Contains("."))
        {
            throw new ArgumentException("Email address must contain at least one '@' and one '.'.");
        }

        Value = value;
    }
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string email) => new Email(email);

    public override string ToString() => Value;
}
