namespace OneToManyApi.Errors;

public class BadRequestException(string message) : Exception(message);
