namespace OnlineSchoolApi.Auth.Dtos;

public record AuthResponse(
    string AccessToken,
    string TokenType,
    long ExpiresInSeconds,
    UserMeResponse User
);
