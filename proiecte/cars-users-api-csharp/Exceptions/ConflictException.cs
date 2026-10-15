namespace OneToManyApi.Exceptions;

public class ConflictException(string message) : ApiException(message, 409);
