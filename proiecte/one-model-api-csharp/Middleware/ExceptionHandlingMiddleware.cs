using System.Text.Json;
using OneModelApi.Exceptions;

namespace OneModelApi.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "Handled API exception for {Method} {Path}. Status: {StatusCode}", context.Request.Method, context.Request.Path, ex.StatusCode);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            var payload = new
            {
                timestamp = DateTimeOffset.UtcNow,
                status = ex.StatusCode,
                error = ex.GetType().Name,
                message = ex.Message,
                path = context.Request.Path.Value
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var payload = new
            {
                timestamp = DateTimeOffset.UtcNow,
                status = StatusCodes.Status500InternalServerError,
                error = "InternalServerError",
                message = "An unexpected error occurred.",
                path = context.Request.Path.Value
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
