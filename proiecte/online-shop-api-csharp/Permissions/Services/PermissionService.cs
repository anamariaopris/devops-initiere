using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Models;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class PermissionService(IPermissionRepository permissionRepository) : IPermissionService
{
    public async Task<List<PermissionResponse>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await permissionRepository.GetAllAsync(cancellationToken);
        return permissions.Select(p => p.ToResponse()).ToList();
    }

    public async Task<PermissionResponse> CreateAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default)
    {
        var name = request.Name!.Trim();
        if (await permissionRepository.GetByNameAsync(name, cancellationToken) is not null)
        {
            throw new PermissionAlreadyExistsException(name);
        }

        var permission = new Permission { Name = name };
        await permissionRepository.AddAsync(permission, cancellationToken);
        await permissionRepository.SaveChangesAsync(cancellationToken);
        return permission.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await permissionRepository.GetByIdAsync(id, cancellationToken)
                         ?? throw new PermissionNotFoundException(id.ToString());
        permissionRepository.Remove(permission);
        await permissionRepository.SaveChangesAsync(cancellationToken);
    }
}
