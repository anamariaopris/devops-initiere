namespace OnlineShopApi.Exceptions;

public class OrderDoesntExistException() : ApiException(404, "Order not found", "Order does not exist in the database.");
