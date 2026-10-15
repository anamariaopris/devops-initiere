using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Common.Errors;
using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Middleware;

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
            logger.LogWarning("Handled API exception for {Method} {Path}: {Message}", context.Request.Method, context.Request.Path, ex.Message);
            await WriteErrorResponseAsync(context, new ApiErrorResponse(ex.StatusCode, ex.Error, ex.Message));
        }
        catch (DbUpdateException ex) when (ex is not DbUpdateConcurrencyException)
        {
            logger.LogWarning(ex, "Database integrity violation for {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorResponseAsync(context, new ApiErrorResponse(
                StatusCodes.Status409Conflict,
                "Data Integrity Violation",
                "Operation violates database integrity constraints"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorResponseAsync(context, new ApiErrorResponse(
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred"));
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, ApiErrorResponse body)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = body.Status;
        await context.Response.WriteAsJsonAsync(body);
    }
}
