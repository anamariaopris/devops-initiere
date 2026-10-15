namespace OnlineShopApi.Exceptions;

public class PermissionAlreadyExistsException(string name) : ApiException(409, "Permission already exists", $"Permission already exists: {name}");
