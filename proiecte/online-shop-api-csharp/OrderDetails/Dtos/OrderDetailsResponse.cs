namespace OnlineShopApi.Dtos;

public record OrderDetailsResponse(Guid Id, int Price, string Sku, int Quantity, ProductsResponse? Product);
