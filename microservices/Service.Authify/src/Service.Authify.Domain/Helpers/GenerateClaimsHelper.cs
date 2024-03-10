﻿using System.Security.Claims;

namespace Service.Authify.Domain.Helpers;

public class GenerateClaimsHelper
{
    public List<Claim> GenerateClaims(string userId, string? role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId)
        };

        if (!string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}