using OnlineShopApi.Validation;

namespace OnlineShopApi.Dtos;

public record CreatePermissionRequest(
    [NotBlank,
     Pattern("^[A-Z][A-Z0-9_]{2,63}$",
         "Permission name must be UPPER_SNAKE_CASE, 3-64 chars, starting with a letter (e.g. CUSTOM_READ).")]
    string? Name
);
