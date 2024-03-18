using Service.Authify.Domain.Exceptions;

namespace Service.Authify.Domain.Helpers;

public class HashHelper : IHashHelper
{
    public string Hash(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new InvalidOperationException("Parameter must not be null or empty.");
        }

        return BCrypt.Net.BCrypt.EnhancedHashPassword(data);
    }

    public bool Verify(string data, string hashedData)
    {
        ArgumentException.ThrowIfNullOrEmpty(data);
        ArgumentException.ThrowIfNullOrEmpty(hashedData);

        bool res;

        try
        {
            res = BCrypt.Net.BCrypt.EnhancedVerify(data, hashedData);
        }
        catch (Exception ex)
        {
            throw new HashVerificationException("Error verifying hashed data.", ex);
        }

        return res;
    }
}