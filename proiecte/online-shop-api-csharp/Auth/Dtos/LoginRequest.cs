using OnlineShopApi.Validation;

namespace OnlineShopApi.Dtos;

public record LoginRequest(
    [NotBlank, Email] string? Email,
    [NotBlank] string? Password
);
