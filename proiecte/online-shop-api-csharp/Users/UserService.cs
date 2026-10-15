using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;

namespace OnlineShopApi.Services;

public class UserService(AppDbContext db)
{
    public async Task<List<UserResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await db.Users.AsNoTracking().Include(u => u.Permissions).ToListAsync(cancellationToken);
        return users.Select(u => u.ToResponse()).ToList();
    }

    public async Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
        (await FindAsync(userId, cancellationToken)).ToResponse();

    public async Task<List<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken) =>
        (await FindAsync(userId, cancellationToken)).SortedPermissionNames();

    public async Task<List<string>> AssignPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken)
    {
        var user = await FindAsync(userId, cancellationToken);
        var permission = await db.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken)
            ?? throw new NotFoundException($"Permission not found: {permissionName}");

        if (user.Permissions.All(p => p.Id != permission.Id))
        {
            user.Permissions.Add(permission);
            await db.SaveChangesAsync(cancellationToken);
        }

        return user.SortedPermissionNames();
    }

    public async Task<List<string>> RevokePermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken)
    {
        var user = await FindAsync(userId, cancellationToken);
        var permission = user.Permissions.FirstOrDefault(p => p.Name == permissionName)
            ?? throw new NotFoundException($"Permission not found: {permissionName} (not held by user)");

        user.Permissions.Remove(permission);
        await db.SaveChangesAsync(cancellationToken);
        return user.SortedPermissionNames();
    }

    private async Task<User> FindAsync(Guid userId, CancellationToken cancellationToken) =>
        await db.Users.Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
        ?? throw new NotFoundException($"User not found: {userId}");
}
