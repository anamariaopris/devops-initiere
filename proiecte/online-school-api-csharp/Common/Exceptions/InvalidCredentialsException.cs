namespace OnlineSchoolApi.Common.Exceptions;

public class InvalidCredentialsException()
    : ApiException("Invalid email or password", StatusCodes.Status401Unauthorized, "Unauthorized");
