using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;
using OnlineShopApi.Security;

namespace OnlineShopApi.Middleware;

public static class ErrorResponses
{
    public const string ForbiddenMessage =
        "You do not have the required permission for this operation. Check your account permissions.";

    public const string UnauthorizedMessage =
        "Authentication required. Add the 'Authorization: Bearer <token>' header, or log in at POST /api/auth/login.";

    private static readonly JsonSerializerOptions JsonOptions = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    public static Dictionary<string, object?> Body(HttpContext context, int status, string error, string? message)
    {
        var body = new Dictionary<string, object?>
        {
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff", CultureInfo.InvariantCulture),
            ["status"] = status,
            ["error"] = error
        };

        if (message is not null)
        {
            body["message"] = message;
        }

        body["path"] = context.Request.Path.Value;
        return body;
    }

    public static Dictionary<string, object?> ForbiddenBody(HttpContext context)
    {
        var body = Body(context, StatusCodes.Status403Forbidden, "Forbidden", ForbiddenMessage);
        body["yourPermissions"] = context.User.GetPermissions();
        return body;
    }

    public static Task WriteAsync(HttpContext context, int status, Dictionary<string, object?> body)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
    }

    public static Task WriteUnauthorizedAsync(HttpContext context)
        => WriteAsync(context, StatusCodes.Status401Unauthorized,
            Body(context, StatusCodes.Status401Unauthorized, "Unauthorized", UnauthorizedMessage));

    public static Task WriteForbiddenAsync(HttpContext context)
        => WriteAsync(context, StatusCodes.Status403Forbidden, ForbiddenBody(context));

    public static Task WriteStatusCodePageAsync(StatusCodeContext statusCodeContext)
    {
        var context = statusCodeContext.HttpContext;
        var status = context.Response.StatusCode;
        return WriteAsync(context, status, Body(context, status, ReasonPhrases.GetReasonPhrase(status), null));
    }
}
