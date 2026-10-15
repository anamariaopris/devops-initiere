namespace OnlineSchoolApi.Common.Exceptions;

public class InvalidRequestException(string message) : BadRequestException(message);
