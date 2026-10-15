using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Models;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class UserService(IUserRepository userRepository, IPermissionRepository permissionRepository) : IUserService
{
    public async Task<List<UserResponse>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return users.Select(u => u.ToResponse()).ToList();
    }

    public async Task<UserResponse> FindByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => (await LoadUserAsync(userId, cancellationToken)).ToResponse();

    public async Task<List<string>> ListPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
        => (await LoadUserAsync(userId, cancellationToken)).SortedPermissionNames();

    public async Task<List<string>> AssignPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    {
        var user = await LoadUserAsync(userId, cancellationToken);
        var permission = await permissionRepository.GetByNameAsync(permissionName, cancellationToken)
                         ?? throw new PermissionNotFoundException(permissionName);

        if (user.Permissions.All(p => p.Id != permission.Id))
        {
            user.Permissions.Add(permission);
            await userRepository.SaveChangesAsync(cancellationToken);
        }

        return user.SortedPermissionNames();
    }

    public async Task<List<string>> RevokePermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    {
        var user = await LoadUserAsync(userId, cancellationToken);
        var held = user.Permissions.Where(p => p.Name == permissionName).ToList();
        if (held.Count == 0)
        {
            throw new PermissionNotFoundException($"{permissionName} (not held by user)");
        }

        foreach (var permission in held)
        {
            user.Permissions.Remove(permission);
        }

        await userRepository.SaveChangesAsync(cancellationToken);
        return user.SortedPermissionNames();
    }

    private async Task<User> LoadUserAsync(Guid userId, CancellationToken cancellationToken)
        => await userRepository.GetByIdAsync(userId, cancellationToken)
           ?? throw new UserNotFoundException(userId.ToString());
}
