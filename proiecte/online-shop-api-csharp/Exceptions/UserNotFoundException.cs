namespace OnlineShopApi.Exceptions;

public class UserNotFoundException(string identifier) : ApiException(404, "User not found", $"User not found: {identifier}");
