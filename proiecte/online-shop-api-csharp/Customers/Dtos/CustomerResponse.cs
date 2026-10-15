namespace OnlineShopApi.Dtos;

public record CustomerResponse(
    Guid Id,
    string Email,
    string FullName,
    string BillingAddress,
    string DefaultShippingAddress,
    string Country,
    string Phone,
    List<OrdersResponse> OrderSet
);
