using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Courses;

public record CourseCreateRequest(
    [Required, MaxLength(255)] string CourseName,
    [Required, MaxLength(255)] string Departament);

public record CourseUpdateRequest(
    [MinLength(1), MaxLength(255)] string? CourseName,
    [MinLength(1), MaxLength(255)] string? Department);

public record CourseResponse(int Id, string CourseName, string Departament);

public static class CourseMappings
{
    public static CourseResponse ToResponse(this Course course) => new(course.Id, course.CourseName, course.Department);
}
