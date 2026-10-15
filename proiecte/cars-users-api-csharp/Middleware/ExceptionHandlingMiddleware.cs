using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using OneToManyApi.Exceptions;

namespace OneToManyApi.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DbUpdateException ex) when (TryMapDbException(ex, out var mappedException))
        {
            logger.LogWarning(ex, "Mapped database exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorResponseAsync(context, mappedException.StatusCode, mappedException.GetType().Name, mappedException.Message);
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "Handled API exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorResponseAsync(context, ex.StatusCode, ex.GetType().Name, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError, "InternalServerError", "An unexpected error occurred.");
        }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string error, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = new
        {
            status = statusCode,
            error,
            message,
            path = context.Request.Path.Value
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }

    private static bool TryMapDbException(DbUpdateException exception, out ApiException mappedException)
    {
        if (exception.InnerException is MySqlException { Number: 1062 })
        {
            mappedException = new ConflictException("Resource already exists.");
            return true;
        }

        if (exception.InnerException is MySqlException { Number: 1452 })
        {
            mappedException = new NotFoundException("Referenced resource was not found.");
            return true;
        }

        mappedException = null!;
        return false;
    }
}
