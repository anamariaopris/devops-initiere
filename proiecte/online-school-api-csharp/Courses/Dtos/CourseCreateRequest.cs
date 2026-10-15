using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Courses.Dtos;

public record CourseCreateRequest(
    [Required] string? CourseName,
    [Required] string? Departament
);
