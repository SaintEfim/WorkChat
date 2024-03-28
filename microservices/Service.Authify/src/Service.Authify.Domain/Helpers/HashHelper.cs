using Microsoft.Extensions.Logging;

namespace Service.Authify.Domain.Helpers;

public class HashHelper : IHashHelper
{
    private readonly ILogger<HashHelper> _logger;

    public HashHelper(ILogger<HashHelper> logger)
    {
        _logger = logger;
    }

    public string Hash(string data)
    {
        if (!string.IsNullOrEmpty(data)) return BCrypt.Net.BCrypt.EnhancedHashPassword(data);
        _logger.LogError("Parameter must not be null or empty.");
        throw new Exception("An error occurred while processing your request.");
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
            _logger.LogError(ex, "Error verifying hashed data.");
            throw new Exception("An error occurred while processing your request.");
        }

        return res;
    }
}