using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;
using OnlineShopApi.Security;

namespace OnlineShopApi.Services;

public class AdminService(AppDbContext db)
{
    public async Task<List<AdminResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var admins = await db.Admins.AsNoTracking().Include(a => a.User).ToListAsync(cancellationToken);
        return admins.Select(a => a.ToResponse()).ToList();
    }

    public async Task<AdminResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        (await FindAsync(a => a.Id == id, cancellationToken)).ToResponse();

    public async Task<AdminResponse> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        (await FindAsync(a => a.User.Email == email, cancellationToken)).ToResponse();

    public async Task<AdminResponse> CreateAsync(CreateAdminRequest request, CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new ConflictException("Email is already in use.");
        }

        var admin = new Admin
        {
            User = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password, AuthService.BCryptWorkFactor),
                Enabled = true,
                Permissions = await db.Permissions.Where(p => Permissions.All.Contains(p.Name)).ToListAsync(cancellationToken)
            },
            DisplayName = request.DisplayName,
            Department = request.Department,
            Notes = request.Notes
        };

        db.Admins.Add(admin);
        await db.SaveChangesAsync(cancellationToken);
        return admin.ToResponse();
    }

    public async Task<AdminResponse> UpdateAsync(Guid id, UpdateAdminRequest request, CancellationToken cancellationToken) =>
        await ApplyAsync(await FindAsync(a => a.Id == id, cancellationToken), request, cancellationToken);

    public async Task<AdminResponse> UpdateByEmailAsync(string email, UpdateAdminRequest request, CancellationToken cancellationToken) =>
        await ApplyAsync(await FindAsync(a => a.User.Email == email, cancellationToken), request, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var admin = await FindAsync(a => a.Id == id, cancellationToken);
        db.Admins.Remove(admin);
        db.Users.Remove(admin.User);
        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task<Admin> FindAsync(Expression<Func<Admin, bool>> predicate, CancellationToken cancellationToken) =>
        await db.Admins.Include(a => a.User).FirstOrDefaultAsync(predicate, cancellationToken)
        ?? throw new NotFoundException("Admin does not exist in the database.");

    private async Task<AdminResponse> ApplyAsync(Admin admin, UpdateAdminRequest request, CancellationToken cancellationToken)
    {
        admin.DisplayName = request.DisplayName;
        admin.Department = request.Department;
        admin.Notes = request.Notes;
        await db.SaveChangesAsync(cancellationToken);
        return admin.ToResponse();
    }
}
