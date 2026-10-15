namespace OnlineShopApi.Dtos;

public record AuthResponse(string AccessToken, string RefreshToken, string Email, List<string> Permissions);
