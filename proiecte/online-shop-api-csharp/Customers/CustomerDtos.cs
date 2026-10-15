using System.ComponentModel.DataAnnotations;
using OnlineShopApi.Models;

namespace OnlineShopApi.Dtos;

public record CustomerDto(
    [Required, MaxLength(255)] string FullName,
    [Required, MaxLength(255)] string BillingAddress,
    [Required, MaxLength(255)] string DefaultShippingAddress,
    [Required, MaxLength(255)] string Country,
    [Required, MaxLength(255)] string Phone);

public record CustomerResponse(
    Guid Id,
    string Email,
    string FullName,
    string BillingAddress,
    string DefaultShippingAddress,
    string Country,
    string Phone,
    List<OrdersResponse> OrderSet);

public record CustomerListResponse(
    List<CustomerResponse> Customers,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize);

public static class CustomerMappings
{
    public static CustomerResponse ToResponse(this Customer c) =>
        new(c.Id, c.User.Email, c.FullName, c.BillingAddress, c.DefaultShippingAddress, c.Country, c.Phone,
            c.Orders.Select(o => o.ToResponse()).ToList());
}
