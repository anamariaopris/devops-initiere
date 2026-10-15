using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace OnlineShopApi.Security;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? throw new InvalidOperationException("Authenticated principal has no email.");

    public static bool HasPermission(this ClaimsPrincipal principal, string permission) =>
        principal.HasClaim(Permissions.ClaimType, permission);
}
