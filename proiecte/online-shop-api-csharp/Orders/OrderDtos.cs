using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OnlineShopApi.Models;

namespace OnlineShopApi.Dtos;

public record OrdersDto(
    [Range(0, 99999999.99)] decimal Ammount,
    [Required, MaxLength(255)] string ShippingAdress,
    [Required, MaxLength(255)] string OrderAddress,
    [Required, MaxLength(255)] string OrderEmail,
    [property: JsonPropertyName("order_date")] [Required] DateOnly? OrderDate,
    [Required, MaxLength(255)] string OrderStatus,
    List<OrderDetailsDto>? OrderDetailsSet);

public record OrderStatusRequest([Required, MaxLength(255)] string Status);

public record OrdersResponse(
    Guid Id,
    decimal Ammount,
    string ShippingAdress,
    string OrderAddress,
    string OrderEmail,
    [property: JsonPropertyName("order_date")] DateOnly OrderDate,
    string OrderStatus,
    List<OrderDetailsResponse> OrderDetailsSet);

public record OrdersListResponse(
    List<OrdersResponse> OrdersList,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize);

public static class OrderMappings
{
    public static OrdersResponse ToResponse(this Order o) =>
        new(o.Id, o.Ammount, o.ShippingAdress, o.OrderAdress, o.OrderEmail, o.OrderDate, o.OrderStatus,
            o.OrderDetails.Select(d => d.ToResponse()).ToList());
}
