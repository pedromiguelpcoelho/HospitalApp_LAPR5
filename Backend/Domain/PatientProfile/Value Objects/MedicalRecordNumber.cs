using DDDSample1.Domain.Shared;

public class MedicalRecordNumber : IValueObject
{
    public string Value { get; private set; }

    // Required for EF Core
    protected MedicalRecordNumber() { }

    public MedicalRecordNumber(string value)
    {

        Value = value;
    }

    public static implicit operator string(MedicalRecordNumber medicalRecordNumber) => medicalRecordNumber.Value;

    public static implicit operator MedicalRecordNumber(string medicalRecordNumber) => new MedicalRecordNumber(medicalRecordNumber);

    public override string ToString() => Value;
}