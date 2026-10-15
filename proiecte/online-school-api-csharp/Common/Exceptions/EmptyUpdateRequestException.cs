namespace OnlineSchoolApi.Common.Exceptions;

public class EmptyUpdateRequestException() : BadRequestException("Update request must contain at least one field to update");
