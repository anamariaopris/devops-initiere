using Microsoft.AspNetCore.Mvc;
using OneToManyApi.Cars;

namespace OneToManyApi.Users;

[ApiController]
[Route("api/users")]
public class UsersController(UserService service, CarService carService) : ControllerBase
{
    [HttpGet]
    public Task<UserWithCarsListResponse> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{id:long}")]
    public Task<UserWithCarsResponse> GetById(long id, CancellationToken cancellationToken) =>
        service.GetByIdAsync(id, cancellationToken);

    [HttpGet("{id:long}/cars")]
    public Task<UserCarsResponse> GetCars(long id, CancellationToken cancellationToken) =>
        carService.GetByUserIdAsync(id, cancellationToken);

    [HttpGet("by-email/{email}")]
    public Task<UserResponse> GetByEmail(string email, CancellationToken cancellationToken) =>
        service.GetByEmailAsync(email, cancellationToken);

    [HttpGet("search")]
    public Task<List<UserResponse>> Search([FromQuery] UserSearchRequest request, CancellationToken cancellationToken) =>
        service.SearchAsync(request, cancellationToken);

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(UserRequest request, CancellationToken cancellationToken)
    {
        var user = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id:long}")]
    public Task<UserResponse> Update(long id, UserRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(id, request, cancellationToken);

    [HttpPatch("{id:long}")]
    public Task<UserResponse> Patch(long id, UserPatchRequest request, CancellationToken cancellationToken) =>
        service.PatchAsync(id, request, cancellationToken);

    [HttpDelete("{id:long}")]
    public Task<UserResponse> Delete(long id, CancellationToken cancellationToken) =>
        service.DeleteAsync(id, cancellationToken);
}
