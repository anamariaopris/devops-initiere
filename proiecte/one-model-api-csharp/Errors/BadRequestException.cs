namespace OneModelApi.Errors;

public class BadRequestException(string message) : Exception(message);
