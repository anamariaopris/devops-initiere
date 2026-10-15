using OnlineShopApi.Validation;

namespace OnlineShopApi.Dtos;

public record AssignPermissionRequest([NotBlank] string? Name);
