using Microsoft.EntityFrameworkCore;
using OneToManyApi.Data;
using OneToManyApi.Dtos;
using OneToManyApi.Models;

namespace OneToManyApi.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Users.AsNoTracking().Include(x => x.Cars).ToListAsync(cancellationToken);

    public Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetByIdWithCarsAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Users.AsNoTracking().Include(x => x.Cars).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<List<User>> SearchAsync(UserSearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            query = query.Where(x => x.Email.Contains(request.Email));
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        => dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public void Remove(User user)
    {
        dbContext.Users.Remove(user);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
