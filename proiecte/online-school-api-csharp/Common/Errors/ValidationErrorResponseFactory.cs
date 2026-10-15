using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineSchoolApi.Common.Validation;

namespace OnlineSchoolApi.Common.Errors;

public static class ValidationErrorResponseFactory
{
    public static IActionResult Create(ActionContext context)
    {
        var parameters = context.ActionDescriptor.Parameters;
        var bodyNames = parameters
            .Where(p => p.BindingInfo?.BindingSource == BindingSource.Body)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var parameterNames = parameters
            .Where(p => p.BindingInfo?.BindingSource != BindingSource.Body)
            .Select(p => p.BindingInfo?.BinderModelName ?? p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var invalid = context.ModelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .Select(entry => (Key: entry.Key, Messages: entry.Value!.Errors.Select(e => e.ErrorMessage).ToList()))
            .ToList();

        var body = Classify(invalid, bodyNames, parameterNames);

        return new ObjectResult(body) { StatusCode = StatusCodes.Status400BadRequest };
    }

    private static ApiErrorResponse Classify(
        List<(string Key, List<string> Messages)> invalid,
        HashSet<string> bodyNames,
        HashSet<string> parameterNames)
    {
        if (invalid.Any(e => e.Key.Length == 0 || e.Key.StartsWith('$') || bodyNames.Contains(e.Key)))
        {
            return new ApiErrorResponse(400, "Malformed Json", ValidationMessages.MalformedBody);
        }

        var parameterErrors = invalid.Where(e => parameterNames.Contains(e.Key)).ToList();

        var mismatch = parameterErrors.FirstOrDefault(e =>
            e.Messages.Any(m => m != ValidationMessages.MissingParameter && m != ValidationMessages.ConstraintFailed));
        if (mismatch.Key is not null)
        {
            return new ApiErrorResponse(400, "Type Mismatch", $"Invalid value for parameter: {mismatch.Key}");
        }

        var missing = parameterErrors.FirstOrDefault(e => e.Messages.Contains(ValidationMessages.MissingParameter));
        if (missing.Key is not null)
        {
            return new ApiErrorResponse(400, "Missing Parameter", $"{ValidationMessages.MissingParameter}: {missing.Key}");
        }

        if (invalid.Any(e => !parameterNames.Contains(e.Key)))
        {
            return new ApiErrorResponse(400, "Validation Failed", ValidationMessages.ValidationFailed);
        }

        return new ApiErrorResponse(400, "Validation Failed", ValidationMessages.ConstraintFailed);
    }
}
