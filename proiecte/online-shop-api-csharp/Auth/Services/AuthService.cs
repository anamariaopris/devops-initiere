using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Models;
using OnlineShopApi.Repositories;
using OnlineShopApi.Security;

namespace OnlineShopApi.Services;

public class AuthService(
    IUserRepository userRepository,
    ICustomerRepository customerRepository,
    IPermissionRepository permissionRepository,
    JwtService jwtService) : IAuthService
{
    public const int BCryptWorkFactor = 10;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email!, cancellationToken))
        {
            throw new EmailAlreadyUsedException();
        }

        var permissions = await permissionRepository.GetByNamesAsync(Permissions.CustomerDefaults, cancellationToken);

        var user = new User
        {
            Email = request.Email!,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password, BCryptWorkFactor),
            Enabled = true,
            Permissions = permissions
        };

        var customer = new Customer
        {
            User = user,
            FullName = request.FullName,
            BillingAddress = request.BillingAddress,
            DefaultShippingAddress = request.DefaultShippingAddress,
            Country = request.Country,
            Phone = request.Phone
        };

        await customerRepository.AddAsync(customer, cancellationToken);
        await customerRepository.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email!, cancellationToken)
                   ?? throw new BadCredentialsException();

        if (!user.Enabled)
        {
            throw new AuthenticationFailedException("User is disabled");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new BadCredentialsException();
        }

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken = default)
    {
        var email = await jwtService.ValidateRefreshTokenAsync(request.RefreshToken!)
                    ?? throw new InvalidRefreshTokenException();

        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
                   ?? throw new AuthenticationFailedException($"User not found: {email}");

        if (!string.Equals(user.Email, email, StringComparison.Ordinal))
        {
            throw new InvalidRefreshTokenException();
        }

        return BuildAuthResponse(user);
    }

    private AuthResponse BuildAuthResponse(User user) =>
        new(jwtService.GenerateAccessToken(user), jwtService.GenerateRefreshToken(user), user.Email, user.SortedPermissionNames());
}
