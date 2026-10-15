namespace OneModelApi.Exceptions;

public class CarAlreadyExistsException(string brand, string model)
    : ApiException($"Car already exists: {brand} {model}", 409);
