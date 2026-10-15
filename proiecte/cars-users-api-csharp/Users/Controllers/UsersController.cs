using Microsoft.AspNetCore.Mvc;
using OneToManyApi.Dtos;
using OneToManyApi.Services;

namespace OneToManyApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserQueryService userQueryService, IUserCommandService userCommandService, ICarQueryService carQueryService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] UserCreateRequest request, CancellationToken cancellationToken)
    {
        var created = await userCommandService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Update([FromRoute] long id, [FromBody] UserUpdateRequest request, CancellationToken cancellationToken)
    {
        var updated = await userCommandService.UpdateAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Patch([FromRoute] long id, [FromBody] UserPatchRequest request, CancellationToken cancellationToken)
    {
        var updated = await userCommandService.PatchAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var deleted = await userCommandService.DeleteAsync(id, cancellationToken);
        return Ok(deleted);
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserWithCarsListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<UserWithCarsListResponse>> GetAll(CancellationToken cancellationToken)
    {
        var users = await userQueryService.GetAllAsync(cancellationToken);
        if (users.Users.Count == 0)
        {
            return NoContent();
        }

        return Ok(users);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserWithCarsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserWithCarsResponse>> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var user = await userQueryService.GetByIdWithCarsAsync(id, cancellationToken);
        return Ok(user);
    }

    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> GetByEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        var user = await userQueryService.GetByEmailAsync(email, cancellationToken);
        return Ok(user);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<List<UserResponse>>> Search([FromQuery] UserSearchRequest request, CancellationToken cancellationToken)
    {
        var users = await userQueryService.SearchAsync(request, cancellationToken);
        if (users.Count == 0)
        {
            return NoContent();
        }

        return Ok(users);
    }

    [HttpGet("{id:long}/cars")]
    [ProducesResponseType(typeof(UserCarsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserCarsResponse>> GetCarsForUser([FromRoute] long id, CancellationToken cancellationToken)
    {
        var cars = await carQueryService.GetByUserIdAsync(id, cancellationToken);
        return Ok(cars);
    }
}
