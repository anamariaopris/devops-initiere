namespace OneToManyApi.Errors;

public class NotFoundException(string message) : Exception(message);
