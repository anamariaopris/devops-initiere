namespace OnlineShopApi.Exceptions;

public abstract class ApiException(int statusCode, string error, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public string Error { get; } = error;
}
