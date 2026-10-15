using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Users.Students.Dtos;

public record StudentCreateRequest(
    [Required, StringLength(50, MinimumLength = 3)] string? FirstName,
    [Required, StringLength(50, MinimumLength = 3)] string? LastName,
    [Required, StringLength(50, MinimumLength = 3)] string? Email
);
