using System.Security.Claims;
using OnlineSchoolApi.Auth.Dtos;

namespace OnlineSchoolApi.Auth.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest? request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(AuthRequest? request, CancellationToken cancellationToken = default);
    Task<UserMeResponse> MeAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
