using Microsoft.AspNetCore.Mvc;
using OnlineSchoolApi.Auth.Dtos;
using OnlineSchoolApi.Auth.Services;

namespace OnlineSchoolApi.Auth.Controllers;

[ApiController]
[Route("auth")]
[Tags("Authentication")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [EndpointSummary("Register a new user")]
    [EndpointDescription("Public endpoint. Creates a student account with the default student permission set and returns a JWT token. No permission required.")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await authService.RegisterAsync(request, cancellationToken));

    [HttpPost("login")]
    [EndpointSummary("Authenticate and receive JWT")]
    [EndpointDescription("Public endpoint. Use one of the bootstrap users shown in the API description. Default password for initialized users: password.")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request, CancellationToken cancellationToken)
        => Ok(await authService.LoginAsync(request, cancellationToken));

    [HttpGet("me")]
    [EndpointSummary("Get current authenticated user")]
    [EndpointDescription("Protected endpoint. Requires a valid Bearer JWT token. Accessible by any authenticated user.")]
    public async Task<ActionResult<UserMeResponse>> Me(CancellationToken cancellationToken)
        => Ok(await authService.MeAsync(User, cancellationToken));
}
