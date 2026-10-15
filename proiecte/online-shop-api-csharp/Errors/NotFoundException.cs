namespace OnlineShopApi.Errors;

public class NotFoundException(string message) : Exception(message);
