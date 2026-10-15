using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OnlineShopApi.Middleware;

public static class ValidationResponseFactory
{
    public static IActionResult Create(ActionContext context)
    {
        var parameterNames = context.ActionDescriptor.Parameters
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var invalid = context.ModelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .ToList();

        var isBindingError = invalid.Any(entry =>
            entry.Key.Length == 0 ||
            entry.Key.StartsWith('$') ||
            parameterNames.Contains(entry.Key));

        if (isBindingError)
        {
            var badRequest = ErrorResponses.Body(context.HttpContext, StatusCodes.Status400BadRequest, "Bad Request", null);
            return new ObjectResult(badRequest) { StatusCode = StatusCodes.Status400BadRequest };
        }

        var body = ErrorResponses.Body(context.HttpContext, StatusCodes.Status400BadRequest, "Validation failed",
            "Request contains invalid fields. See details below.");
        body["errors"] = invalid.ToDictionary(
            entry => JsonNamingPolicy.CamelCase.ConvertName(entry.Key),
            entry => FirstMessage(entry.Value!));

        return new ObjectResult(body) { StatusCode = StatusCodes.Status400BadRequest };
    }

    private static string FirstMessage(ModelStateEntry entry) => entry.Errors[0].ErrorMessage;
}
