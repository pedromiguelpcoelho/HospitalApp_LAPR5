using System;
using DDDSample1.Domain.Shared;

public class ContactInformation : IValueObject
{
    public long Value { get; set; }
    
    public ContactInformation() { }

    public ContactInformation(long value) {
        var valueString = value.ToString();
        if (valueString.Length != 9 || valueString[0] != '9') {
            throw new ArgumentException("Phone number must have exactly 9 digits and start with 9.");
        }
        this.Value = value;
    }

    public static implicit operator long(ContactInformation number) => number.Value;
    public static implicit operator ContactInformation(long number) => new ContactInformation(number);

    public override string ToString() => Value.ToString();
}