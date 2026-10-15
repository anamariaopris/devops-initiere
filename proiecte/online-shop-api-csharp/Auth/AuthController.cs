using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AuthController(AuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken) =>
        StatusCode(StatusCodes.Status201Created, await service.RegisterAsync(request, cancellationToken));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await service.LoginAsync(request, cancellationToken);
        if (response is null)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: "Invalid email or password.");
        }

        return response;
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh(RefreshRequest request, CancellationToken cancellationToken)
    {
        var response = await service.RefreshAsync(request, cancellationToken);
        if (response is null)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: "Refresh token is invalid or expired.");
        }

        return response;
    }
}
