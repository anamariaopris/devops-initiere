using System.Text.Json.Serialization;
using OnlineShopApi.Common;

namespace OnlineShopApi.Dtos;

public record ProductsDto(
    string? Sku,
    string? Name,
    int Price,
    int Weight,
    string? Descriptions,
    string? Category,
    [property: JsonConverter(typeof(JavaDateConverter))] DateTime? CreateDate,
    int Stock
);
