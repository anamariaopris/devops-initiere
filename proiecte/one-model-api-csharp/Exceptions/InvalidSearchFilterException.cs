namespace OneModelApi.Exceptions;

public class InvalidSearchFilterException(string message)
    : ApiException(message, 400);
