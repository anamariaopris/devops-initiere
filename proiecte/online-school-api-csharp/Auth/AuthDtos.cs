using System.ComponentModel.DataAnnotations;
using OnlineSchoolApi.Users;

namespace OnlineSchoolApi.Auth;

public record RegisterRequest(
    [Required, StringLength(50, MinimumLength = 3)] string FirstName,
    [Required, StringLength(50, MinimumLength = 3)] string LastName,
    [Required, EmailAddress, StringLength(50, MinimumLength = 3)] string Email,
    [Required, StringLength(255, MinimumLength = 8)] string Password,
    string? PhoneNumber);

public record AuthRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public record UserMeResponse(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    bool Active,
    List<string> Permissions);

public record AuthResponse(
    string AccessToken,
    string TokenType,
    long ExpiresInSeconds,
    UserMeResponse User);

public static class AuthMappings
{
    public static UserMeResponse ToMeResponse(this User user) => new(
        user.Id, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.IsActive, user.GetAuthorities());
}
