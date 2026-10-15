using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace OnlineShopApi.Dtos;

public record OrdersDto(
    int Ammount,
    string? ShippingAdress,
    string? OrderAddress,
    string? OrderEmail,
    [property: JsonPropertyName("order_date")] DateOnly? OrderDate,
    string? OrderStatus,
    [ValidateNever] List<OrderDetailsDto>? OrderDetailsSet
);
