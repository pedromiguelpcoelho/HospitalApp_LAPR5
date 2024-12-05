using System;
using DDDSample1.Domain.Shared;
using System.Text.RegularExpressions;

public class PhoneNumber : IValueObject
{
    public string Value { get; private set; }

    protected PhoneNumber()
    {
    }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Phone number cannot be null or empty.");
        }

        // Validar que o número de telefone contém apenas dígitos
        if (!Regex.IsMatch(value, @"^\d+$"))
        {
            throw new ArgumentException("Phone number can only contain digits.");
        }

     // Verifica se o valor contém exatamente 9 dígitos e começa com 9
    if (value.Length != 9 || value[0] != '9') {
        throw new ArgumentException("Phone number must have exactly 9 digits and start with 9.");
    }


        Value = value;
    }

    // Conversão implícita de PhoneNumber para string
    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;

    // Conversão implícita de string para PhoneNumber
    public static implicit operator PhoneNumber(string phoneNumber) => new PhoneNumber(phoneNumber);

    public override string ToString() => Value;
}
