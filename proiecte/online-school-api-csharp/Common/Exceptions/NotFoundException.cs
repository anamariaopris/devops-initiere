namespace OnlineSchoolApi.Common.Exceptions;

public abstract class NotFoundException(string message) : ApiException(message, StatusCodes.Status404NotFound, "Not Found");
