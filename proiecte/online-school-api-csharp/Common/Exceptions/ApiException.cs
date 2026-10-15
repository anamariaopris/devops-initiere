namespace OnlineSchoolApi.Common.Exceptions;

public abstract class ApiException : Exception
{
    public int StatusCode { get; }
    public string Error { get; }

    protected ApiException(string message, int statusCode, string error) : base(message)
    {
        StatusCode = statusCode;
        Error = error;
    }
}
