namespace OnlineSchoolApi.Common.Exceptions;

public class TypeMismatchException(string parameterName)
    : ApiException($"Invalid value for parameter: {parameterName}", StatusCodes.Status400BadRequest, "Type Mismatch");
