using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Models;
using OnlineShopApi.Repositories;
using OnlineShopApi.Security;

namespace OnlineShopApi.Services;

public class AdminService(
    IAdminRepository adminRepository,
    IUserRepository userRepository,
    IPermissionRepository permissionRepository) : IAdminService
{
    public async Task<List<AdminResponse>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        var admins = await adminRepository.GetAllAsync(cancellationToken);
        return admins.Select(a => a.ToResponse()).ToList();
    }

    public async Task<AdminResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await LoadAsync(id, cancellationToken)).ToResponse();

    public async Task<AdminResponse> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        => (await LoadByEmailAsync(email, cancellationToken)).ToResponse();

    public async Task<AdminResponse> CreateAsync(CreateAdminRequest request, CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email!, cancellationToken))
        {
            throw new EmailAlreadyUsedException();
        }

        var permissions = await permissionRepository.GetByNamesAsync(Permissions.All, cancellationToken);

        var admin = new Admin
        {
            User = new User
            {
                Email = request.Email!,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password, AuthService.BCryptWorkFactor),
                Enabled = true,
                Permissions = permissions
            },
            DisplayName = request.DisplayName!,
            Department = request.Department,
            Notes = request.Notes
        };

        await adminRepository.AddAsync(admin, cancellationToken);
        await adminRepository.SaveChangesAsync(cancellationToken);
        return admin.ToResponse();
    }

    public async Task<AdminResponse> UpdateAsync(Guid id, UpdateAdminRequest request, CancellationToken cancellationToken = default)
        => await ApplyAndSaveAsync(await LoadAsync(id, cancellationToken), request, cancellationToken);

    public async Task<AdminResponse> UpdateByEmailAsync(string email, UpdateAdminRequest request, CancellationToken cancellationToken = default)
        => await ApplyAndSaveAsync(await LoadByEmailAsync(email, cancellationToken), request, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var admin = await LoadAsync(id, cancellationToken);
        adminRepository.Remove(admin);
        await adminRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<AdminResponse> ApplyAndSaveAsync(Admin admin, UpdateAdminRequest request, CancellationToken cancellationToken)
    {
        admin.DisplayName = request.DisplayName!;
        admin.Department = request.Department;
        admin.Notes = request.Notes;
        await adminRepository.SaveChangesAsync(cancellationToken);
        return admin.ToResponse();
    }

    private async Task<Admin> LoadAsync(Guid id, CancellationToken cancellationToken)
        => await adminRepository.GetByIdAsync(id, cancellationToken) ?? throw new AdminNotFoundException();

    private async Task<Admin> LoadByEmailAsync(string email, CancellationToken cancellationToken)
        => await adminRepository.GetByUserEmailAsync(email, cancellationToken) ?? throw new AdminNotFoundException();
}
