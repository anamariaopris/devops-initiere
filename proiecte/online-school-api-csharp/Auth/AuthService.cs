using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;
using OnlineSchoolApi.Users;
using OnlineSchoolApi.Users.Students;

namespace OnlineSchoolApi.Auth;

public class AuthService(AppDbContext db, JwtService jwtService, IConfiguration configuration)
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new ConflictException($"Student already exists with email={request.Email}");
        }

        var student = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, configuration.GetValue("Security:PasswordEncoderStrength", 10)),
            Permissions = UserPermission.Grant(PermissionTemplates.Student)
        };
        db.Students.Add(student);
        await db.SaveChangesAsync(cancellationToken);
        return ToAuthResponse(student);
    }

    public async Task<AuthResponse?> LoginAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        var user = await db.Users.Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user is not { IsActive: true, PasswordHash: not null } || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        user.LastLoginAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
        return ToAuthResponse(user);
    }

    public async Task<UserMeResponse> MeAsync(ClaimsPrincipal principal, CancellationToken cancellationToken)
    {
        int.TryParse(principal.FindFirstValue(JwtService.UserIdClaim), out var userId);
        var user = await db.Users.AsNoTracking().Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new NotFoundException("Authenticated user not found");
        return user.ToMeResponse();
    }

    private AuthResponse ToAuthResponse(User user) =>
        new(jwtService.CreateToken(user), "Bearer", jwtService.ExpirationSeconds, user.ToMeResponse());
}
