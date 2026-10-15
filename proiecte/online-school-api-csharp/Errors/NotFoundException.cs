namespace OnlineSchoolApi.Errors;

public class NotFoundException(string message) : Exception(message);
