namespace OnlineShopApi.Exceptions;

public class BadCredentialsException() : ApiException(401, "Bad credentials", "Invalid email or password.");
