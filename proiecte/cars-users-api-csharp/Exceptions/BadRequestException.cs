namespace OneToManyApi.Exceptions;

public class BadRequestException(string message) : ApiException(message, 400);
