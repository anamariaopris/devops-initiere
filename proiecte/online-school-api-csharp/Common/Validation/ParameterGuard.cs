using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Common.Validation;

public static class ParameterGuard
{
    public static string NotBlank(string? value)
        => string.IsNullOrWhiteSpace(value) ? throw new ConstraintViolationException() : value;
}
