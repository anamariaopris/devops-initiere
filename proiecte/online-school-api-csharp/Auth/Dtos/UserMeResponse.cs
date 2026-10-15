namespace OnlineSchoolApi.Auth.Dtos;

public record UserMeResponse(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    bool Active,
    List<string> Permissions
);
