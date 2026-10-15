namespace OnlineSchoolApi.Common.Exceptions;

public abstract class ConflictException(string message) : ApiException(message, StatusCodes.Status409Conflict, "Conflict");
