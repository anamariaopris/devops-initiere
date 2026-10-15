using Microsoft.EntityFrameworkCore;
using OneToManyApi.Data;
using OneToManyApi.Errors;

namespace OneToManyApi.Users;

public class UserService(AppDbContext db)
{
    public async Task<UserWithCarsListResponse> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await db.Users.AsNoTracking().Include(u => u.Cars.OrderBy(c => c.Id)).OrderBy(u => u.Id).ToListAsync(cancellationToken);
        return new UserWithCarsListResponse(users.Select(u => u.ToWithCarsResponse()));
    }

    public async Task<UserWithCarsResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var user = await db.Users.AsNoTracking().Include(u => u.Cars.OrderBy(c => c.Id)).FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw NotFound(id);
        return user.ToWithCarsResponse();
    }

    public async Task<UserResponse> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
            ?? throw new NotFoundException($"User not found with email {email}");
        return user.ToResponse();
    }

    public async Task<List<UserResponse>> SearchAsync(UserSearchRequest request, CancellationToken cancellationToken)
    {
        var query = db.Users.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Name)) query = query.Where(u => u.Name.Contains(request.Name));
        if (!string.IsNullOrWhiteSpace(request.Email)) query = query.Where(u => u.Email.Contains(request.Email));
        var users = await query.OrderBy(u => u.Id).ToListAsync(cancellationToken);
        return users.Select(u => u.ToResponse()).ToList();
    }

    public async Task<UserResponse> CreateAsync(UserRequest request, CancellationToken cancellationToken)
    {
        await EnsureEmailAvailableAsync(request.Email, null, cancellationToken);
        var user = new User { Name = request.Name, Email = request.Email };
        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return user.ToResponse();
    }

    public async Task<UserResponse> UpdateAsync(long id, UserRequest request, CancellationToken cancellationToken)
    {
        var user = await FindAsync(id, cancellationToken);
        await EnsureEmailAvailableAsync(request.Email, id, cancellationToken);
        user.Name = request.Name;
        user.Email = request.Email;
        await db.SaveChangesAsync(cancellationToken);
        return user.ToResponse();
    }

    public async Task<UserResponse> PatchAsync(long id, UserPatchRequest request, CancellationToken cancellationToken)
    {
        var user = await FindAsync(id, cancellationToken);
        if (request is { Name: null, Email: null })
        {
            throw new BadRequestException("Patch request must contain at least one field");
        }

        if (request.Email is not null)
        {
            await EnsureEmailAvailableAsync(request.Email, id, cancellationToken);
        }

        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        await db.SaveChangesAsync(cancellationToken);
        return user.ToResponse();
    }

    public async Task<UserResponse> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var user = await FindAsync(id, cancellationToken);
        db.Users.Remove(user);
        await db.SaveChangesAsync(cancellationToken);
        return user.ToResponse();
    }

    private async Task<User> FindAsync(long id, CancellationToken cancellationToken) =>
        await db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken) ?? throw NotFound(id);

    private async Task EnsureEmailAvailableAsync(string email, long? excludedId, CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(u => u.Email == email && u.Id != excludedId, cancellationToken))
        {
            throw new ConflictException($"User email already exists: {email}");
        }
    }

    private static NotFoundException NotFound(long id) => new($"User not found with id {id}");
}
