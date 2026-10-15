namespace OnlineShopApi.Exceptions;

public class InvalidRefreshTokenException() : ApiException(401, "Invalid refresh token", "Refresh token is invalid or expired.");
