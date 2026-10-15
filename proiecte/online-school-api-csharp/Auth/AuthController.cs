using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineSchoolApi.Auth;

[ApiController]
[Route("auth")]
public class AuthController(AuthService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken) =>
        StatusCode(StatusCodes.Status201Created, await service.RegisterAsync(request, cancellationToken));

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request, CancellationToken cancellationToken)
    {
        var response = await service.LoginAsync(request, cancellationToken);
        return response is null
            ? Problem(statusCode: StatusCodes.Status401Unauthorized, detail: "Invalid email or password")
            : Ok(response);
    }

    [HttpGet("me")]
    public Task<UserMeResponse> Me(CancellationToken cancellationToken) =>
        service.MeAsync(User, cancellationToken);
}
