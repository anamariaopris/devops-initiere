using Microsoft.AspNetCore.Authentication.JwtBearer;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Security;

public static class PrincipalLoader
{
    public static async Task LoadFromDatabaseAsync(TokenValidatedContext context)
    {
        var email = context.Principal?.FindFirst(ClaimsPrincipalExtensions.EmailClaim)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            context.Fail("Token has no subject.");
            return;
        }

        var users = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var user = await users.GetByEmailAsync(email, context.HttpContext.RequestAborted);
        if (user is null || !string.Equals(user.Email, email, StringComparison.Ordinal))
        {
            context.Fail("User not found.");
            return;
        }

        context.Principal = user.ToPrincipal(JwtBearerDefaults.AuthenticationScheme);
    }
}
