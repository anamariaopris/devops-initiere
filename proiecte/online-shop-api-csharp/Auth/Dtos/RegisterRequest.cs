using OnlineShopApi.Validation;

namespace OnlineShopApi.Dtos;

public record RegisterRequest(
    [NotBlank, Email] string? Email,
    [NotBlank, Size(8, 100)] string? Password,
    [NotBlank] string? FullName,
    [NotBlank] string? BillingAddress,
    [NotBlank] string? DefaultShippingAddress,
    [NotBlank] string? Country,
    [NotBlank] string? Phone
);
