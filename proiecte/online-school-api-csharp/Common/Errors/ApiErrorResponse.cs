namespace OnlineSchoolApi.Common.Errors;

public record ApiErrorResponse(int Status, string Error, string Message);
