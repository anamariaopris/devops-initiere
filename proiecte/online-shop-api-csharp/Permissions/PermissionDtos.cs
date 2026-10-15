using System.ComponentModel.DataAnnotations;
using OnlineShopApi.Models;

namespace OnlineShopApi.Dtos;

public record CreatePermissionRequest(
    [Required]
    [RegularExpression("^[A-Z][A-Z0-9_]{2,63}$",
        ErrorMessage = "Permission name must be UPPER_SNAKE_CASE, 3-64 chars, starting with a letter (e.g. CUSTOM_READ).")]
    string Name);

public record PermissionResponse(Guid Id, string Name);

public static class PermissionMappings
{
    public static PermissionResponse ToResponse(this Permission p) => new(p.Id, p.Name);
}
