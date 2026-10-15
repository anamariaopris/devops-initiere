namespace OneModelApi.Errors;

public class NotFoundException(string message) : Exception(message);
