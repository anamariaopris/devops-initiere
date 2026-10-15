namespace OnlineShopApi.Dtos;

public record AdminResponse(Guid Id, string Email, string DisplayName, string? Department, string? Notes);
