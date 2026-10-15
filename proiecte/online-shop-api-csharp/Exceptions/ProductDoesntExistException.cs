namespace OnlineShopApi.Exceptions;

public class ProductDoesntExistException() : ApiException(404, "Product not found", "Product does not exist in the database.");
