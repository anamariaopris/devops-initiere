using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Auth.Dtos;

public record AuthRequest(
    [Required, EmailAddress] string? Email,
    [Required] string? Password
);
