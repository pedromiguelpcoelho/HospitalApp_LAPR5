using System;
using DDDSample1.Domain.Shared;

public class LicenseNumber : IValueObject
{
    public string Value { get; private set; }

    protected LicenseNumber()
    {
    }

    public LicenseNumber(string role, int sequentialNumber)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role cannot be null or empty.");
        }

        if (sequentialNumber <= 0)
        {
            throw new ArgumentException("Sequential number must be greater than zero.");
        }

        Value = GenerateLicenseNumber(role, sequentialNumber);
    }

    // Método para gerar o número de licença
    private static string GenerateLicenseNumber(string role, int sequentialNumber)
    {
        string licenseType = GetLicenseTypeFromRole(role);
        int currentYear = DateTime.Now.Year;

        // O número sequencial tem um padding de 5 dígitos
        string licenseNumber = $"{licenseType}{currentYear}{sequentialNumber:D5}";

        return licenseNumber;
    }

    // Determina o tipo de licença com base no papel
    private static string GetLicenseTypeFromRole(string role)
    {
        return role switch
        {
            "Doctor" => "D",
            "Nurse" => "N",
            _ => "O" // O para outros papéis
        };
    }

    // Conversão implícita de LicenseNumber para string
    public static implicit operator string(LicenseNumber licenseNumber) => licenseNumber.Value;

    // Conversão implícita de string para LicenseNumber
    public static implicit operator LicenseNumber((string role, int sequentialNumber) data) 
        => new LicenseNumber(data.role, data.sequentialNumber);

    public override string ToString() => Value;
}
