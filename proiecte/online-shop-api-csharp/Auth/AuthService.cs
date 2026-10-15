using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;
using OnlineShopApi.Security;

namespace OnlineShopApi.Services;

public class AuthService(AppDbContext db, JwtService jwtService)
{
    public const int BCryptWorkFactor = 10;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new ConflictException("Email is already in use.");
        }

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password, BCryptWorkFactor),
            Enabled = true,
            Permissions = await db.Permissions.Where(p => Permissions.CustomerDefaults.Contains(p.Name)).ToListAsync(cancellationToken)
        };

        db.Customers.Add(new Customer
        {
            User = user,
            FullName = request.FullName,
            BillingAddress = request.BillingAddress,
            DefaultShippingAddress = request.DefaultShippingAddress,
            Country = request.Country,
            Phone = request.Phone
        });
        await db.SaveChangesAsync(cancellationToken);

        return ToAuthResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await FindUserAsync(request.Email, cancellationToken);
        if (user is null || !user.Enabled || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return null;
        }

        return ToAuthResponse(user);
    }

    public async Task<AuthResponse?> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken)
    {
        var email = await jwtService.ValidateRefreshTokenAsync(request.RefreshToken);
        var user = email is null ? null : await FindUserAsync(email, cancellationToken);
        return user is null || !user.Enabled ? null : ToAuthResponse(user);
    }

    private Task<User?> FindUserAsync(string email, CancellationToken cancellationToken) =>
        db.Users.Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    private AuthResponse ToAuthResponse(User user) =>
        new(jwtService.GenerateAccessToken(user), jwtService.GenerateRefreshToken(user), user.Email, user.SortedPermissionNames());
}
