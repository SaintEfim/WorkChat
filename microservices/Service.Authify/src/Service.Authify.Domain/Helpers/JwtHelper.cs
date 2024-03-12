using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Service.Authify.Domain.Exceptions;

namespace Service.Authify.Domain.Helpers;

public class JwtHelper : IJwtHelper
{
    public async Task<string> GenerateToken(string userId, string? role, string secretKey, TimeSpan expiresIn,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException($"{userId} or {secretKey} must not be null or empty.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = await GenerateKey(secretKey, cancellationToken).ConfigureAwait(false);

        if (key == null)
        {
            throw new KeyGenerationException("Failed to generate key.");
        }

        var claims = await GenerateClaims(userId, role, cancellationToken).ConfigureAwait(false);

        if (claims == null)
        {
            throw new ClaimsGenerationException("Failed to generate claims.");
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(expiresIn),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<ClaimsPrincipal> DecodeToken(string token, string secretKey,
        CancellationToken cancellationToken = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(await GenerateKey(secretKey, cancellationToken));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        return principal;
    }

    private static Task<byte[]> GenerateKey(string secretKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(Encoding.UTF8.GetBytes(secretKey));
    }

    private static Task<List<Claim>> GenerateClaims(string userId, string? role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId)
        };

        if (!string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return Task.FromResult(claims);
    }
}