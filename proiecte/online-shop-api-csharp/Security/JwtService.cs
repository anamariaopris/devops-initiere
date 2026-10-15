using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OnlineShopApi.Models;

namespace OnlineShopApi.Security;

public class JwtService(JwtOptions options)
{
    private const string TypeClaim = "type";
    private readonly JsonWebTokenHandler _handler = new();
    private readonly SymmetricSecurityKey _key = CreateSigningKey(options);
    private readonly TokenValidationParameters _validationParameters = CreateValidationParameters(options);

    public string GenerateAccessToken(User user)
    {
        var claims = new Dictionary<string, object>
        {
            ["authorities"] = user.Permissions.Select(p => p.Name).Order(StringComparer.Ordinal).ToList(),
            [TypeClaim] = "access"
        };
        return BuildToken(claims, user.Email, options.AccessTokenExpirationMs);
    }

    public string GenerateRefreshToken(User user)
    {
        var claims = new Dictionary<string, object> { [TypeClaim] = "refresh" };
        return BuildToken(claims, user.Email, options.RefreshTokenExpirationMs);
    }

    public async Task<string?> ValidateRefreshTokenAsync(string token)
    {
        var result = await _handler.ValidateTokenAsync(token, _validationParameters);
        if (!result.IsValid)
        {
            return null;
        }

        var isRefresh = result.Claims.TryGetValue(TypeClaim, out var type) && "refresh".Equals(type as string);
        if (!isRefresh)
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
        IssuerSigningKey = CreateSigningKey(options),
        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
        ClockSkew = TimeSpan.Zero
    };

    private string BuildToken(Dictionary<string, object> claims, string subject, long expirationMs)
    {
        var now = DateTime.UtcNow;
        claims[JwtRegisteredClaimNames.Sub] = subject;
        var descriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMilliseconds(expirationMs),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
        };
        return _handler.CreateToken(descriptor);
    }

    private static SymmetricSecurityKey CreateSigningKey(JwtOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Secret))
        {
            throw new InvalidOperationException("Jwt:Secret is missing.");
        }

        var bytes = TryDecodeBase64(options.Secret) ?? Encoding.UTF8.GetBytes(options.Secret);
        if (bytes.Length < 32)
        {
            throw new InvalidOperationException("Jwt:Secret must be at least 256 bits (32 bytes) for HS256.");
        }

        return new SymmetricSecurityKey(bytes);
    }

    private static byte[]? TryDecodeBase64(string value)
    {
        var buffer = new byte[value.Length];
        return Convert.TryFromBase64String(value, buffer, out var written) ? buffer[..written] : null;
    }
}
