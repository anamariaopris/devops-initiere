using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OnlineShopApi.Dtos;
using OnlineShopApi.Models;

namespace OnlineShopApi.Security;

public class JwtService(IOptions<JwtOptions> jwtOptions)
{
    public const string TypeClaim = "type";
    public const string AccessType = "access";
    public const string RefreshType = "refresh";

    private readonly JwtOptions options = jwtOptions.Value;
    private readonly JsonWebTokenHandler handler = new();

    public string GenerateAccessToken(User user)
    {
        var claims = new Dictionary<string, object>
        {
            [Permissions.ClaimType] = user.SortedPermissionNames(),
            [TypeClaim] = AccessType
        };
        return BuildToken(claims, user.Email, options.AccessTokenExpirationMs);
    }

    public string GenerateRefreshToken(User user)
    {
        var claims = new Dictionary<string, object> { [TypeClaim] = RefreshType };
        return BuildToken(claims, user.Email, options.RefreshTokenExpirationMs);
    }

    public async Task<string?> ValidateRefreshTokenAsync(string token)
    {
        var result = await handler.ValidateTokenAsync(token, CreateValidationParameters(options));
        if (!result.IsValid || !result.Claims.TryGetValue(TypeClaim, out var type) || !RefreshType.Equals(type))
        {
            return null;
        }

        return result.Claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var subject) ? subject as string : null;
    }

    public static TokenValidationParameters CreateValidationParameters(JwtOptions options) => new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(SecretBytes(options.Secret)),
        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
        NameClaimType = JwtRegisteredClaimNames.Sub,
        ClockSkew = TimeSpan.Zero
    };

    public static byte[] SecretBytes(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            return [];
        }

        var buffer = new byte[secret.Length];
        return Convert.TryFromBase64String(secret, buffer, out var written) ? buffer[..written] : Encoding.UTF8.GetBytes(secret);
    }

    private string BuildToken(Dictionary<string, object> claims, string subject, long expirationMs)
    {
        var now = DateTime.UtcNow;
        claims[JwtRegisteredClaimNames.Sub] = subject;
        return handler.CreateToken(new SecurityTokenDescriptor
        {
            Claims = claims,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMilliseconds(expirationMs),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretBytes(options.Secret)), SecurityAlgorithms.HmacSha256)
        });
    }
}
