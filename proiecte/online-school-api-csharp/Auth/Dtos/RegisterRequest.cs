using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Auth.Dtos;

public record RegisterRequest(
    [Required, StringLength(50, MinimumLength = 3)] string? FirstName,
    [Required, StringLength(50, MinimumLength = 3)] string? LastName,
    [Required, EmailAddress, StringLength(50, MinimumLength = 3)] string? Email,
    [Required, StringLength(255, MinimumLength = 8)] string? Password,
    string? PhoneNumber
);
