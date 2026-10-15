using System.ComponentModel.DataAnnotations;
using OnlineShopApi.Models;

namespace OnlineShopApi.Dtos;

public record ProductsDto(
    [Required, MaxLength(255)] string Sku,
    [Required, MaxLength(255)] string Name,
    [Range(0, 99999999.99)] decimal Price,
    int Weight,
    [Required, MaxLength(255)] string Descriptions,
    [Required, MaxLength(255)] string Category,
    DateTime? CreateDate,
    int Stock);

public record ProductsResponse(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    int Weight,
    string Descriptions,
    string Category,
    DateTime CreateDate,
    int Stock);

public record ProductsListResponse(
    List<ProductsResponse> ProductsList,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize);

public static class ProductMappings
{
    public static ProductsResponse ToResponse(this Product p) =>
        new(p.Id, p.Sku, p.Name, p.Price, p.Weight, p.Descriptions, p.Category, p.CreateDate, p.Stock);
}
