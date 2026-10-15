namespace OnlineSchoolApi.Common.Exceptions;

public abstract class BadRequestException(string message) : ApiException(message, StatusCodes.Status400BadRequest, "Bad Request");
