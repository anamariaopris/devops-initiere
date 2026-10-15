using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Enrolments.Exceptions;

public class EnrolmentAlreadyExistsException(long studentId, long courseId)
    : ConflictException($"Enrolment already exists for studentId={studentId} and courseId={courseId}");
