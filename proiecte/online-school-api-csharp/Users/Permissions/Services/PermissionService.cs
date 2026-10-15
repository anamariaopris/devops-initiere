using OnlineSchoolApi.Users.Exceptions;
using OnlineSchoolApi.Users.Models;
using OnlineSchoolApi.Users.Permissions.Dtos;
using OnlineSchoolApi.Users.Repositories;

namespace OnlineSchoolApi.Users.Permissions.Services;

public class PermissionService(IUserRepository userRepository) : IPermissionService
{
    public List<PermissionResponse> GetAllPermissions() => Enum.GetValues<Permission>()
        .OrderBy(p => p.ToString(), StringComparer.Ordinal)
        .Select(p => new PermissionResponse(p.ToString(), p.ToAuthority()))
        .ToList();

    public async Task<UserPermissionsResponse> GetUserPermissionsAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithPermissionsAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        return ToResponse(user);
    }

    public async Task<UserPermissionsResponse> AddPermissionsAsync(long userId, IEnumerable<Permission> permissions, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetTrackedByIdWithPermissionsAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        foreach (var permission in permissions.Distinct())
        {
            if (user.Permissions.All(p => p.Permission != permission))
            {
                user.Permissions.Add(new UserPermission { UserId = user.Id, Permission = permission });
            }
        }

        await userRepository.SaveChangesAsync(cancellationToken);
        return ToResponse(user);
    }

    public async Task<UserPermissionsResponse> RemovePermissionAsync(long userId, Permission permission, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetTrackedByIdWithPermissionsAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        var existing = user.Permissions.FirstOrDefault(p => p.Permission == permission);
        if (existing is not null)
        {
            user.Permissions.Remove(existing);
            await userRepository.SaveChangesAsync(cancellationToken);
        }

        return ToResponse(user);
    }

    private static UserPermissionsResponse ToResponse(User user) => new(user.Id, user.GetAuthorities());
}
