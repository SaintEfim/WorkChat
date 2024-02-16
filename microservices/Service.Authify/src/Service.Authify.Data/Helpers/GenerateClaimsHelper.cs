using System.Security.Claims;

namespace Service.Authify.Data.Helpers;

internal class GenerateClaimsHelper
{
    public List<Claim> GenerateClaims(string userId, string? role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userId)
        };

        if (!string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}