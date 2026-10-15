using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Enrolments.Exceptions;

public class EnrolmentNotFoundException(long enrolmentId) : NotFoundException($"Enrolment not found with id={enrolmentId}");
