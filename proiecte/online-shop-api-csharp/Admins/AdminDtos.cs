using System.ComponentModel.DataAnnotations;
using OnlineShopApi.Models;

namespace OnlineShopApi.Dtos;

public record CreateAdminRequest(
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, StringLength(100, MinimumLength = 8)] string Password,
    [Required, MaxLength(255)] string DisplayName,
    [MaxLength(255)] string? Department,
    [MaxLength(1000)] string? Notes);

public record UpdateAdminRequest(
    [Required, MaxLength(255)] string DisplayName,
    [MaxLength(255)] string? Department,
    [MaxLength(1000)] string? Notes);

public record AdminResponse(Guid Id, string Email, string DisplayName, string? Department, string? Notes);

public static class AdminMappings
{
    public static AdminResponse ToResponse(this Admin a) => new(a.Id, a.User.Email, a.DisplayName, a.Department, a.Notes);
}
