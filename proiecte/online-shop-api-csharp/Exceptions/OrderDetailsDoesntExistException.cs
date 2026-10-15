namespace OnlineShopApi.Exceptions;

public class OrderDetailsDoesntExistException() : ApiException(404, "Order details not found", "Order detail does not exist in the database.");
