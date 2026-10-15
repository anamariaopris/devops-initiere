using OnlineShopApi.Exceptions;

namespace OnlineShopApi.Middleware;

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
            await ErrorResponses.WriteAsync(context, ex.StatusCode, ErrorResponses.Body(context, ex.StatusCode, ex.Error, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await ErrorResponses.WriteAsync(context, StatusCodes.Status500InternalServerError,
                ErrorResponses.Body(context, StatusCodes.Status500InternalServerError, "Internal Server Error", null));
        }
    }
}
