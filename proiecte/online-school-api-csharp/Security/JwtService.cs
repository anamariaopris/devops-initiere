using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OnlineSchoolApi.Users.Models;

namespace OnlineSchoolApi.Security;

public class JwtService(IOptions<JwtOptions> options)
{
    public long ExpirationSeconds => options.Value.ExpirationSeconds;

    public string GenerateToken(User user)
    {
        var key = CreateSigningKey(options.Value.Secret);
        var now = DateTime.UtcNow;
        var descriptor = new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Email,
                ["userId"] = user.Id,
                ["permissions"] = user.Permissions
                    .OrderBy(p => p.Permission)
                    .Select(p => p.Permission.ToAuthority())
                    .ToList()
            },
            IssuedAt = now,
            Expires = now.AddSeconds(options.Value.ExpirationSeconds),
            SigningCredentials = new SigningCredentials(key, SelectAlgorithm(key))
        };

        var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
        return handler.CreateToken(descriptor);
    }

    public static SymmetricSecurityKey CreateSigningKey(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("Jwt:Secret is missing.");
        }

        var keyBytes = Convert.FromBase64String(secret);
        if (keyBytes.Length < 32)
        {
            throw new InvalidOperationException("Jwt:Secret must be Base64 and decode to at least 32 bytes.");
        }

        return new SymmetricSecurityKey(keyBytes);
    }

    private static string SelectAlgorithm(SymmetricSecurityKey key) => key.Key.Length switch
    {
        >= 64 => SecurityAlgorithms.HmacSha512,
        >= 48 => SecurityAlgorithms.HmacSha384,
        _ => SecurityAlgorithms.HmacSha256
    };

    public static ClaimsPrincipal BuildPrincipal(User user, string authenticationType)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Email)
        };
        claims.AddRange(user.Permissions.Select(p => new Claim(SecurityConstants.AuthorityClaimType, p.Permission.ToAuthority())));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType));
    }
}
