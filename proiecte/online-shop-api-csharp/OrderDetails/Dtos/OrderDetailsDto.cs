using System.ComponentModel.DataAnnotations;

namespace OnlineShopApi.Dtos;

public record OrderDetailsDto(
    int Price,
    string? Sku,
    int Quantity,
    [Required(ErrorMessage = "Product ID is required")] Guid? ProductId
);
