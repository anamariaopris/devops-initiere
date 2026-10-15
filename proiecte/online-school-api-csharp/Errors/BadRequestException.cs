namespace OnlineSchoolApi.Errors;

public class BadRequestException(string message) : Exception(message)
{
    public static BadRequestException EmptyUpdate() => new("Update request must contain at least one field to update");
}
