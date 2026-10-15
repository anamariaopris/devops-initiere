namespace OneModelApi.Exceptions;

public class CarNotFoundException(long id) : ApiException($"Car not found with id {id}", 404);
