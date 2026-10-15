using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Users.AsNoTracking().Include(x => x.Permissions).ToListAsync(cancellationToken);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Users.Include(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => dbContext.Users.Include(x => x.Permissions).FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        => dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await dbContext.Users.AddAsync(user, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
