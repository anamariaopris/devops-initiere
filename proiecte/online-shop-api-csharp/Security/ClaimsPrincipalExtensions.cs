using System.Security.Claims;
using OnlineShopApi.Models;

namespace OnlineShopApi.Security;

public static class ClaimsPrincipalExtensions
{
    public const string EmailClaim = "sub";
    public const string PermissionClaim = "permission";

    public static ClaimsPrincipal ToPrincipal(this User user, string authenticationType)
    {
        var claims = new List<Claim> { new(EmailClaim, user.Email) };
        claims.AddRange(user.Permissions.Select(p => new Claim(PermissionClaim, p.Name)));
        var identity = new ClaimsIdentity(claims, authenticationType, EmailClaim, PermissionClaim);
        return new ClaimsPrincipal(identity);
    }

    public static string GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(EmailClaim) ?? throw new InvalidOperationException("Authenticated principal has no email.");

    public static List<string> GetPermissions(this ClaimsPrincipal principal)
        => principal.FindAll(PermissionClaim).Select(c => c.Value).Order(StringComparer.Ordinal).ToList();

    public static bool HasPermission(this ClaimsPrincipal principal, string permission)
        => principal.HasClaim(PermissionClaim, permission);
}
