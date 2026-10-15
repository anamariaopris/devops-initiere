using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShopApi.Middleware;

namespace OnlineShopApi.Security;

[AttributeUsage(AttributeTargets.Method)]
public class RequirePermissionAttribute(string permission) : Attribute, IAsyncActionFilter
{
    public OwnedResource Owner { get; set; } = OwnedResource.None;
    public string OwnerArgument { get; set; } = "id";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!await IsAllowedAsync(context))
        {
            var body = ErrorResponses.ForbiddenBody(context.HttpContext);
            context.Result = new ObjectResult(body) { StatusCode = StatusCodes.Status403Forbidden };
            return;
        }

        await next();
    }

    private async Task<bool> IsAllowedAsync(ActionExecutingContext context)
    {
        var principal = context.HttpContext.User;
        if (principal.HasPermission(permission))
        {
            return true;
        }

        if (Owner == OwnedResource.None ||
            !context.ActionArguments.TryGetValue(OwnerArgument, out var argument) ||
            argument is not Guid resourceId)
        {
            return false;
        }

        var security = context.HttpContext.RequestServices.GetRequiredService<ISecurityService>();
        var email = principal.GetEmail();
        var cancellationToken = context.HttpContext.RequestAborted;

        return Owner switch
        {
            OwnedResource.Customer => await security.IsCustomerOwnerAsync(resourceId, email, cancellationToken),
            OwnedResource.Order => await security.IsOrderOwnerAsync(resourceId, email, cancellationToken),
            _ => false
        };
    }
}
