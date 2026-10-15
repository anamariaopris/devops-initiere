namespace OnlineShopApi.Exceptions;

public class EmailAlreadyUsedException() : ApiException(409, "Email already used", "Email is already in use.");
