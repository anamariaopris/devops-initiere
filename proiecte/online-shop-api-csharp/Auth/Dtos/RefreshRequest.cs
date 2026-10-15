using OnlineShopApi.Validation;

namespace OnlineShopApi.Dtos;

public record RefreshRequest([NotBlank] string? RefreshToken);
