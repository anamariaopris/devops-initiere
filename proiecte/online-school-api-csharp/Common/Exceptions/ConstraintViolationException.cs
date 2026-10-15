using OnlineSchoolApi.Common.Validation;

namespace OnlineSchoolApi.Common.Exceptions;

public class ConstraintViolationException()
    : ApiException(ValidationMessages.ConstraintFailed, StatusCodes.Status400BadRequest, "Validation Failed");
