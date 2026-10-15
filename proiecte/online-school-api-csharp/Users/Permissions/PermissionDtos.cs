using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Users.Permissions;

public record PermissionResponse(string Name, string Authority);

public record UpdateUserPermissionsRequest([Required, MinLength(1)] List<Permission> Permissions);

public record UserPermissionsResponse(long UserId, List<string> Permissions);
