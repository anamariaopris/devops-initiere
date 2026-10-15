namespace OnlineShopApi.Dtos;

public record UserResponse(Guid Id, string Email, bool Enabled, List<string> Permissions);
