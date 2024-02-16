using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Service.Authify.Data.Helpers;

public class GenerateTokenHelper
{
    private readonly GenerateClaimsHelper _generateClaimsHelper;
    private readonly GenerateKeyHelper _generateKeyHelper;

    GenerateTokenHelper(GenerateClaimsHelper generateClaimsHelper, GenerateKeyHelper generateKeyHelper)
    {
        _generateClaimsHelper = generateClaimsHelper;
        _generateKeyHelper = generateKeyHelper;
    }

    public string GenerateToken(string userId, string? role, string secretKey, TimeSpan expiresIn)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentException("userId and secretKey must not be null or empty.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _generateKeyHelper.GenerateKey(secretKey);

        if (key == null)
        {
            throw new Exception("The generated key is null.");
        }

        var claims = _generateClaimsHelper.GenerateClaims(userId, role);

        if (claims == null)
        {
            throw new Exception("The generated key is null.");
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(expiresIn),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}