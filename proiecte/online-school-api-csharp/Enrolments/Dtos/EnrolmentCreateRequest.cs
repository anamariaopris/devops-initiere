using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Enrolments.Dtos;

public record EnrolmentCreateRequest(
    [Required] long? StudentId,
    [Required] long? CourseId
);
