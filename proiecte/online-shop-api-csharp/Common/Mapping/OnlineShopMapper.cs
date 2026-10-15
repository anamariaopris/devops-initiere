using OnlineShopApi.Dtos;
using OnlineShopApi.Models;

namespace OnlineShopApi.Common;

public static class OnlineShopMapper
{
    private static string Nvl(string? value) => value ?? string.Empty;
    private static string? Trim(string? value) => value?.Trim();

    public static ProductsResponse ToResponse(this Product e) =>
        new(e.Id, Nvl(e.Sku), Nvl(e.Name), e.Price, e.Weight, Nvl(e.Descriptions), Nvl(e.Category), e.CreateDate, e.Stock);

    public static Product ToEntity(this ProductsDto dto) => new()
    {
        Sku = Trim(dto.Sku),
        Name = Trim(dto.Name),
        Price = dto.Price,
        Weight = dto.Weight,
        Descriptions = Trim(dto.Descriptions),
        Category = Trim(dto.Category),
        CreateDate = dto.CreateDate,
        Stock = dto.Stock
    };

    public static OrderDetailsResponse ToResponse(this OrderDetail e) =>
        new(e.Id, e.Price, Nvl(e.Sku), e.Quantity, e.Product?.ToResponse());

    public static OrderDetail ToEntity(this OrderDetailsDto dto) => new()
    {
        Price = dto.Price,
        Sku = Trim(dto.Sku),
        Quantity = dto.Quantity
    };

    public static OrdersResponse ToResponse(this Order e) =>
        new(e.Id, e.Ammount, Nvl(e.ShippingAdress), Nvl(e.OrderAdress), Nvl(e.OrderEmail), e.OrderDate, Nvl(e.OrderStatus),
            e.OrderDetails.Select(d => d.ToResponse()).ToList());

    public static Order ToEntity(this OrdersDto dto) => new()
    {
        Ammount = dto.Ammount,
        ShippingAdress = Trim(dto.ShippingAdress),
        OrderAdress = Trim(dto.OrderAddress),
        OrderEmail = Trim(dto.OrderEmail),
        OrderDate = dto.OrderDate,
        OrderStatus = Trim(dto.OrderStatus)
    };

    public static CustomerResponse ToResponse(this Customer e) =>
        new(e.Id, Nvl(e.User?.Email), Nvl(e.FullName), Nvl(e.BillingAddress), Nvl(e.DefaultShippingAddress), Nvl(e.Country), Nvl(e.Phone),
            e.Orders.Select(o => o.ToResponse()).ToList());

    public static AdminResponse ToResponse(this Admin e) =>
        new(e.Id, e.User?.Email ?? string.Empty, e.DisplayName, e.Department, e.Notes);

    public static PermissionResponse ToResponse(this Permission e) => new(e.Id, e.Name);

    public static List<string> SortedPermissionNames(this User user) =>
        user.Permissions.Select(p => p.Name).Order(StringComparer.Ordinal).ToList();

    public static UserResponse ToResponse(this User user) => new(user.Id, user.Email, user.Enabled, user.SortedPermissionNames());
}
