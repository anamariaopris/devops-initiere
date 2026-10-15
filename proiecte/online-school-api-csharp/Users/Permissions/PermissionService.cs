using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;

namespace OnlineSchoolApi.Users.Permissions;

public class PermissionService(AppDbContext db)
{
    public List<PermissionResponse> GetAll() => Enum.GetValues<Permission>()
        .OrderBy(p => p.ToString(), StringComparer.Ordinal)
        .Select(p => new PermissionResponse(p.ToString(), p.ToAuthority()))
        .ToList();

    public async Task<UserPermissionsResponse> GetForUserAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await FindAsync(userId, cancellationToken);
        return ToResponse(user);
    }

    public async Task<UserPermissionsResponse> AddAsync(long userId, List<Permission> permissions, CancellationToken cancellationToken)
    {
        var user = await FindAsync(userId, cancellationToken);
        foreach (var permission in permissions.Distinct().Where(p => user.Permissions.All(up => up.Permission != p)))
        {
            user.Permissions.Add(new UserPermission { Permission = permission });
        }

        await db.SaveChangesAsync(cancellationToken);
        return ToResponse(user);
    }

    public async Task<UserPermissionsResponse> RemoveAsync(long userId, Permission permission, CancellationToken cancellationToken)
    {
        var user = await FindAsync(userId, cancellationToken);
        var existing = user.Permissions.FirstOrDefault(p => p.Permission == permission);
        if (existing is not null)
        {
            user.Permissions.Remove(existing);
            await db.SaveChangesAsync(cancellationToken);
        }
        return ToResponse(user);
    }

    private async Task<User> FindAsync(long userId, CancellationToken cancellationToken) =>
        await db.Users.Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new NotFoundException($"user with id {userId} not found");

    private static UserPermissionsResponse ToResponse(User user) => new(user.Id, user.GetAuthorities());
}
