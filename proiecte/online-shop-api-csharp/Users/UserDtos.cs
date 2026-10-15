using System.ComponentModel.DataAnnotations;
using OnlineShopApi.Models;

namespace OnlineShopApi.Dtos;

public record AssignPermissionRequest([Required] string Name);

public record UserResponse(Guid Id, string Email, bool Enabled, List<string> Permissions);

public static class UserMappings
{
    public static List<string> SortedPermissionNames(this User user) =>
        user.Permissions.Select(p => p.Name).Order(StringComparer.Ordinal).ToList();

    public static UserResponse ToResponse(this User user) => new(user.Id, user.Email, user.Enabled, user.SortedPermissionNames());
}
