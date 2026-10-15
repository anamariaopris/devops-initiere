namespace OneToManyApi.Errors;

public class ConflictException(string message) : Exception(message);
