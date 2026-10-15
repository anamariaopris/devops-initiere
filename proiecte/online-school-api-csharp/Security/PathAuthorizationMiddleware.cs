using Microsoft.Extensions.Options;
using OnlineSchoolApi.Common.Errors;

namespace OnlineSchoolApi.Security;

public class PathAuthorizationMiddleware
{
    private static readonly string[] WriteMethods = [HttpMethods.Post, HttpMethods.Put, HttpMethods.Patch, HttpMethods.Delete];

    private readonly RequestDelegate _next;
    private readonly List<PathPattern> _publicPaths;
    private readonly List<(PathPattern Pattern, ResourceRule Rule)> _rules;

    public PathAuthorizationMiddleware(RequestDelegate next, IOptions<SecurityOptions> options)
    {
        _next = next;
        _publicPaths = options.Value.PublicPaths.Select(p => new PathPattern(p)).ToList();
        _rules = options.Value.ResourceRules.Select(r => (new PathPattern(r.PathPattern), r)).ToList();
    }

    public async Task Invoke(HttpContext context)
    {
        var path = ResolvePath(context);

        if (_publicPaths.Any(p => p.Matches(path)))
        {
            await _next(context);
            return;
        }

        var requiredAuthority = FindRequiredAuthority(context.Request.Method, path);
        var authenticated = context.User.Identity?.IsAuthenticated == true;

        if (!authenticated)
        {
            await WriteAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized", "Authentication is required to access this resource");
            return;
        }

        if (requiredAuthority is not null && !context.User.HasClaim(SecurityConstants.AuthorityClaimType, requiredAuthority))
        {
            await WriteAsync(context, StatusCodes.Status403Forbidden, "Forbidden", "You do not have permission to access this resource");
            return;
        }

        await _next(context);
    }

    private string? FindRequiredAuthority(string method, string path)
    {
        var isRead = HttpMethods.IsGet(method);
        var isWrite = WriteMethods.Any(m => string.Equals(m, method, StringComparison.OrdinalIgnoreCase));
        if (!isRead && !isWrite)
        {
            return null;
        }

        var match = _rules.FirstOrDefault(r => r.Pattern.Matches(path));
        if (match.Rule is null)
        {
            return null;
        }

        return isRead ? match.Rule.ReadAuthority : match.Rule.WriteAuthority;
    }

    private static string ResolvePath(HttpContext context)
    {
        if (context.GetEndpoint() is RouteEndpoint endpoint && endpoint.RoutePattern.RawText is { } template)
        {
            return "/" + template.TrimStart('/');
        }

        return context.Request.Path.HasValue ? context.Request.Path.Value! : "/";
    }

    private static Task WriteAsync(HttpContext context, int status, string error, string message)
    {
        context.Response.StatusCode = status;
        return context.Response.WriteAsJsonAsync(new ApiErrorResponse(status, error, message));
    }
}
