using System.Text.Json.Serialization;

namespace OnlineShopApi.Dtos;

public record OrdersResponse(
    Guid Id,
    int Ammount,
    string ShippingAdress,
    string OrderAddress,
    string OrderEmail,
    [property: JsonPropertyName("order_date")] DateOnly? OrderDate,
    string OrderStatus,
    List<OrderDetailsResponse> OrderDetailsSet
);
