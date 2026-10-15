using System.ComponentModel.DataAnnotations;
using OnlineShopApi.Models;

namespace OnlineShopApi.Dtos;

public record OrderDetailsDto(
    [Range(0, 99999999.99)] decimal Price,
    [Required, MaxLength(255)] string Sku,
    int Quantity,
    [Required(ErrorMessage = "Product ID is required")] Guid? ProductId);

public record OrderDetailsResponse(Guid Id, decimal Price, string Sku, int Quantity, ProductsResponse? Product);

public record OrderDetailsListResponse(
    List<OrderDetailsResponse> OrderDetailsList,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize);

public static class OrderDetailMappings
{
    public static OrderDetailsResponse ToResponse(this OrderDetail d) =>
        new(d.Id, d.Price, d.Sku, d.Quantity, d.Product?.ToResponse());

    public static OrderDetail ToEntity(this OrderDetailsDto request, Product product) => new()
    {
        Price = request.Price,
        Sku = request.Sku.Trim(),
        Quantity = request.Quantity,
        Product = product
    };
}
