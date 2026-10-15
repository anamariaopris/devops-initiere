using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace OneToManyApi.Errors;

public class ApiExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<ApiExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int? status = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            BadRequestException => StatusCodes.Status400BadRequest,
            DbUpdateException { InnerException: MySqlException { ErrorCode: MySqlErrorCode.DuplicateKeyEntry } } => StatusCodes.Status409Conflict,
            _ => null
        };

        if (status is null)
        {
            logger.LogError(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
            return false;
        }

        httpContext.Response.StatusCode = status.Value;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = { Status = status, Detail = exception is DbUpdateException ? "A record with the same unique value already exists." : exception.Message }
        });
    }
}
