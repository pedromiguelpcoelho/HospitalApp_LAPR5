using System;
using DDDSample1.Domain.Shared;

public class Role : IValueObject
{
    public string Value { get; private set; }

    protected Role()
    {
    }

    public Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Role cannot be null or empty.");
        }

        if (!IsValidRole(value))
        {
            throw new ArgumentException($"Invalid role: {value}. Valid roles are: Doctor, Nurse, Other.");
        }

        Value = value;
    }

    // Método para verificar se o papel é válido
    private static bool IsValidRole(string role)
    {
        return role == "Doctor" || role == "Nurse" || role == "Other";
    }

    // Conversão implícita de Role para string
    public static implicit operator string(Role role) => role.Value;

    // Conversão implícita de string para Role
    public static implicit operator Role(string role) => new Role(role);

    public override string ToString() => Value;
}
