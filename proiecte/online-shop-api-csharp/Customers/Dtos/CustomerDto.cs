namespace OnlineShopApi.Dtos;

public record CustomerDto(
    string? FullName,
    string? BillingAddress,
    string? DefaultShippingAddress,
    string? Country,
    string? Phone
);
