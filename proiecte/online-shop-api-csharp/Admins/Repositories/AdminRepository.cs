using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public class AdminRepository(AppDbContext dbContext) : IAdminRepository
{
    public Task<List<Admin>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Admins.AsNoTracking().Include(x => x.User).ToListAsync(cancellationToken);

    public Task<Admin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Admins.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Admin?> GetByUserEmailAsync(string email, CancellationToken cancellationToken = default)
        => dbContext.Admins.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Email == email, cancellationToken);

    public async Task AddAsync(Admin admin, CancellationToken cancellationToken = default)
        => await dbContext.Admins.AddAsync(admin, cancellationToken);

    public void Remove(Admin admin)
    {
        dbContext.Admins.Remove(admin);
        dbContext.Users.Remove(admin.User);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
