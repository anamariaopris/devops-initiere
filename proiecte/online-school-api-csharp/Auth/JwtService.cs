using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OnlineSchoolApi.Users;

namespace OnlineSchoolApi.Auth;

public class JwtService(IOptions<JwtOptions> options)
{
    public const string UserIdClaim = "userId";
    public const string PermissionsClaim = "permissions";

    public long ExpirationSeconds => options.Value.ExpirationSeconds;

    public string CreateToken(User user)
    {
        var key = SigningKey();
        var now = DateTime.UtcNow;
        var descriptor = new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Email,
                [UserIdClaim] = user.Id,
                [PermissionsClaim] = user.Permissions.OrderBy(p => p.Permission).Select(p => p.Permission.ToAuthority()).ToList()
            },
            IssuedAt = now,
            Expires = now.AddSeconds(options.Value.ExpirationSeconds),
            SigningCredentials = new SigningCredentials(key, Algorithm(key))
        };
        return new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false }.CreateToken(descriptor);
    }

    public TokenValidationParameters ValidationParameters() => new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SigningKey(),
        ClockSkew = TimeSpan.Zero
    };

    private SymmetricSecurityKey SigningKey() => new(options.Value.KeyBytes());

    private static string Algorithm(SymmetricSecurityKey key) => key.Key.Length switch
    {
        >= 64 => SecurityAlgorithms.HmacSha512,
        >= 48 => SecurityAlgorithms.HmacSha384,
        _ => SecurityAlgorithms.HmacSha256
    };
}
