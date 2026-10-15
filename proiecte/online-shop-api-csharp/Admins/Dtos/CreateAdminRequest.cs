using OnlineShopApi.Validation;

namespace OnlineShopApi.Dtos;

public record CreateAdminRequest(
    [NotBlank, Email] string? Email,
    [NotBlank, Size(8, 100)] string? Password,
    [NotBlank] string? DisplayName,
    string? Department,
    string? Notes
);
