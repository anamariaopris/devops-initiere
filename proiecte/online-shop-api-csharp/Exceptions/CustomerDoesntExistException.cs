namespace OnlineShopApi.Exceptions;

public class CustomerDoesntExistException() : ApiException(404, "Customer not found", "Customer does not exist in the database.");
