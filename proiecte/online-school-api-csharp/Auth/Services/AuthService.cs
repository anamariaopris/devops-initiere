using System.Security.Claims;
using Microsoft.Extensions.Options;
using OnlineSchoolApi.Auth.Dtos;
using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Security;
using OnlineSchoolApi.Users.Models;
using OnlineSchoolApi.Users.Repositories;
using OnlineSchoolApi.Users.Students.Exceptions;
using OnlineSchoolApi.Users.Students.Models;

namespace OnlineSchoolApi.Auth.Services;

public class AuthService(
    IUserRepository userRepository,
    JwtService jwtService,
    IOptions<SecurityOptions> securityOptions) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest? request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new InvalidRequestException("request body must not be null");
        }

        var email = request.Email ?? string.Empty;
        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new StudentAlreadyExistsException(email);
        }

        var student = new Student
        {
            FirstName = request.FirstName ?? string.Empty,
            LastName = request.LastName ?? string.Empty,
            Email = email,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, securityOptions.Value.PasswordEncoderStrength),
            IsActive = true,
            Permissions = PermissionTemplates.StudentDefaults()
                .Select(p => new UserPermission { Permission = p })
                .ToList()
        };

        await userRepository.AddAsync(student, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(student);
    }

    public async Task<AuthResponse> LoginAsync(AuthRequest? request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new InvalidRequestException("request body must not be null");
        }

        var user = await userRepository.GetTrackedByEmailWithPermissionsAsync(request.Email ?? string.Empty, cancellationToken);
        if (user is null || !user.IsActive || !PasswordMatches(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        user.LastLoginAt = DateTime.Now;
        await userRepository.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(user);
    }

    public async Task<UserMeResponse> MeAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var idValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(idValue, out var userId))
        {
            throw new InvalidRequestException("authenticated user not found");
        }

        var user = await userRepository.GetByIdWithPermissionsAsync(userId, cancellationToken)
            ?? throw new InvalidRequestException("authenticated user not found");

        return ToMeResponse(user);
    }

    private static bool PasswordMatches(string? password, string? passwordHash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordHash))
        {
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
    }

    private AuthResponse BuildAuthResponse(User user) => new(
        jwtService.GenerateToken(user),
        "Bearer",
        jwtService.ExpirationSeconds,
        ToMeResponse(user)
    );

    private static UserMeResponse ToMeResponse(User user) => new(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        user.PhoneNumber,
        user.IsActive,
        user.GetAuthorities()
    );
}
