using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Users.Students;

public record StudentCreateRequest(
    [Required, StringLength(50, MinimumLength = 3)] string FirstName,
    [Required, StringLength(50, MinimumLength = 3)] string LastName,
    [Required, EmailAddress, StringLength(50, MinimumLength = 3)] string Email);

public record StudentUpdateRequest(
    [StringLength(50, MinimumLength = 3)] string? FirstName,
    [StringLength(50, MinimumLength = 3)] string? LastName,
    [EmailAddress, StringLength(50, MinimumLength = 3)] string? Email);

public record StudentResponse(int Id, string FirstName, string LastName, string Email);

public static class StudentMappings
{
    public static StudentResponse ToResponse(this Student student) =>
        new(student.Id, student.FirstName, student.LastName, student.Email);
}
