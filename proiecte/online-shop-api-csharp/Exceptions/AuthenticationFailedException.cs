namespace OnlineShopApi.Exceptions;

public class AuthenticationFailedException(string message) : ApiException(401, "Unauthorized", message);
