using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Patients;

public class TokenManager : ITokenManager
{
    private readonly Dictionary<string, (string Email, DateTime Expiry)> _tokens = new();

    public void SetToken(string token, string email, TimeSpan expiry)
    {
        _tokens[token] = (email, DateTime.UtcNow.Add(expiry));
    }

    public bool TryGetToken(string token, out string email)
    {
        if (_tokens.TryGetValue(token, out var value) && value.Expiry > DateTime.UtcNow)
        {
            email = value.Email;
            return true;
        }

        email = null;
        return false;
    }

    public void RemoveToken(string token)
    {
        _tokens.Remove(token);
    }
}