using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Enrolments;

public record EnrolmentCreateRequest(
    [Range(1, long.MaxValue)] long StudentId,
    [Range(1, long.MaxValue)] long CourseId);

public record EnrolmentUpdateRequest(long? StudentId, long? CourseId);

public record EnrolmentResponse(int StudentId, int CourseId);

public static class EnrolmentMappings
{
    public static EnrolmentResponse ToResponse(this Enrolment enrolment) =>
        new(enrolment.StudentId.GetValueOrDefault(), enrolment.CourseId.GetValueOrDefault());
}
