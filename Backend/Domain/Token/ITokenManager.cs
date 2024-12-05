using System;

namespace DDDSample1.Domain.Patients;

public interface ITokenManager
{
    void SetToken(string token, string email, TimeSpan duration);
    bool TryGetToken(string token, out string email);
    void RemoveToken(string token);
}