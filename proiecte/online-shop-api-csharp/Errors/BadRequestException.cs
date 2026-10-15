namespace OnlineShopApi.Errors;

public class BadRequestException(string message) : Exception(message);
