namespace OnlineShopApi.Exceptions;

public class PermissionNotFoundException(string identifier) : ApiException(404, "Permission not found", $"Permission not found: {identifier}");
