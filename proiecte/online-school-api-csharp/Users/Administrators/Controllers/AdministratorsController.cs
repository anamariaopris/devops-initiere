using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Users.Administrators.Dtos;
using OnlineSchoolApi.Users.Administrators.Services;

namespace OnlineSchoolApi.Users.Administrators.Controllers;

[ApiController]
[Route("administrators")]
[Tags("Administrators")]
public class AdministratorsController(IAdministratorQueryService administratorQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all administrators")]
    [EndpointDescription("Required permission: administrator:read")]
    public async Task<ActionResult<List<AdministratorResponse>>> GetAllAdministrators(CancellationToken cancellationToken)
        => Ok(await administratorQueryService.GetAllAdministratorsAsync(cancellationToken));

    [HttpGet("{administratorId}")]
    [EndpointSummary("Get administrator by id")]
    [EndpointDescription("Required permission: administrator:read")]
    public async Task<ActionResult<AdministratorResponse>> GetAdministratorById([FromRoute, Positive] long administratorId, CancellationToken cancellationToken)
        => Ok(await administratorQueryService.GetAdministratorByIdAsync(administratorId, cancellationToken));

    [HttpGet("find-by-email")]
    [EndpointSummary("Find administrator by email")]
    [EndpointDescription("Required permission: administrator:read")]
    public async Task<ActionResult<AdministratorResponse>> FindByEmail([FromQuery, BindRequired] string? email, CancellationToken cancellationToken)
        => Ok(await administratorQueryService.FindByEmailAsync(ParameterGuard.NotBlank(email), cancellationToken));

    [HttpGet("search")]
    [EndpointSummary("Search administrators")]
    [EndpointDescription("Required permission: administrator:read")]
    public async Task<ActionResult<List<AdministratorResponse>>> SearchAdministrators([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken)
        => Ok(await administratorQueryService.SearchAdministratorsAsync(query, cancellationToken));
}
