using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;

namespace OnlineSchoolApi.Users.Administrators;

public class AdministratorService(AppDbContext db)
{
    public Task<List<AdministratorResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Administrators, cancellationToken);

    public async Task<AdministratorResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var administrator = await db.Administrators.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new NotFoundException($"administrator with id {id} not found");
        return administrator.ToResponse();
    }

    public async Task<AdministratorResponse> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var administrator = await db.Administrators.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email, cancellationToken)
            ?? throw new NotFoundException($"administrator with email {email} not found");
        return administrator.ToResponse();
    }

    public async Task<List<AdministratorResponse>> SearchAsync(string? query, CancellationToken cancellationToken)
    {
        var term = query?.Trim();
        if (string.IsNullOrEmpty(term))
        {
            return [];
        }
        return await ListAsync(db.Administrators.Where(a =>
            a.FirstName.Contains(term) || a.LastName.Contains(term) || a.Email.Contains(term)), cancellationToken);
    }

    private static async Task<List<AdministratorResponse>> ListAsync(IQueryable<Administrator> query, CancellationToken cancellationToken)
    {
        var administrators = await query.AsNoTracking().OrderBy(a => a.Id).ToListAsync(cancellationToken);
        return administrators.Select(a => a.ToResponse()).ToList();
    }
}
