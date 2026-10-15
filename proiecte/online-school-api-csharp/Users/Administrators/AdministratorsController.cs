using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineSchoolApi.Users.Administrators;

[ApiController]
[Route("administrators")]
public class AdministratorsController(AdministratorService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "administrator:read")]
    public Task<List<AdministratorResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{administratorId}")]
    [Authorize(Policy = "administrator:read")]
    public Task<AdministratorResponse> GetById([Range(1, long.MaxValue)] long administratorId, CancellationToken cancellationToken) =>
        service.GetByIdAsync(administratorId, cancellationToken);

    [HttpGet("find-by-email")]
    [Authorize(Policy = "administrator:read")]
    public Task<AdministratorResponse> FindByEmail([Required] string email, CancellationToken cancellationToken) =>
        service.FindByEmailAsync(email, cancellationToken);

    [HttpGet("search")]
    [Authorize(Policy = "administrator:read")]
    public Task<List<AdministratorResponse>> Search([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken) =>
        service.SearchAsync(query, cancellationToken);
}
