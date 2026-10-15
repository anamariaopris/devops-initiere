using System.ComponentModel.DataAnnotations;

namespace OnlineShopApi.Dtos;

public record RegisterRequest(
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, StringLength(100, MinimumLength = 8)] string Password,
    [Required, MaxLength(255)] string FullName,
    [Required, MaxLength(255)] string BillingAddress,
    [Required, MaxLength(255)] string DefaultShippingAddress,
    [Required, MaxLength(255)] string Country,
    [Required, MaxLength(255)] string Phone);

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public record RefreshRequest([Required] string RefreshToken);

public record AuthResponse(string AccessToken, string RefreshToken, string Email, List<string> Permissions);
