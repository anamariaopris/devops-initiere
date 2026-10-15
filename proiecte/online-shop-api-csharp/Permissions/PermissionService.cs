using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;

namespace OnlineShopApi.Services;

public class PermissionService(AppDbContext db)
{
    public async Task<List<PermissionResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var permissions = await db.Permissions.AsNoTracking().ToListAsync(cancellationToken);
        return permissions.Select(p => p.ToResponse()).ToList();
    }

    public async Task<PermissionResponse> CreateAsync(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        if (await db.Permissions.AnyAsync(p => p.Name == request.Name, cancellationToken))
        {
            throw new ConflictException($"Permission already exists: {request.Name}");
        }

        var permission = new Permission { Name = request.Name };
        db.Permissions.Add(permission);
        await db.SaveChangesAsync(cancellationToken);
        return permission.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var permission = await db.Permissions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new NotFoundException($"Permission not found: {id}");
        db.Permissions.Remove(permission);
        await db.SaveChangesAsync(cancellationToken);
    }
}
