using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using OnlineShopApi.Middleware;

namespace OnlineShopApi.Security;

public class JsonAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            await ErrorResponses.WriteUnauthorizedAsync(context);
            return;
        }

        if (authorizeResult.Forbidden)
        {
            await ErrorResponses.WriteForbiddenAsync(context);
            return;
        }

        await next(context);
    }
}
