using OnlineShopApi.Validation;

namespace OnlineShopApi.Dtos;

public record UpdateAdminRequest(
    [NotBlank] string? DisplayName,
    string? Department,
    string? Notes
);
