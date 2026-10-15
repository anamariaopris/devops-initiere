using OnlineSchoolApi.Enrolments.Dtos;
using OnlineSchoolApi.Enrolments.Models;

namespace OnlineSchoolApi.Enrolments.Mappers;

public static class EnrolmentMapper
{
    public static EnrolmentResponse ToResponse(Enrolment enrolment) => new(
        enrolment.StudentId ?? throw new InvalidOperationException("Enrolment has no student"),
        enrolment.CourseId ?? throw new InvalidOperationException("Enrolment has no course")
    );
}
