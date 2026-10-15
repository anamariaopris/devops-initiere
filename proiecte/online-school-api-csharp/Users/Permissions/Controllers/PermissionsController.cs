using Microsoft.AspNetCore.Mvc;
using OnlineSchoolApi.Users.Permissions.Dtos;
using OnlineSchoolApi.Users.Permissions.Services;

namespace OnlineSchoolApi.Users.Permissions.Controllers;

[ApiController]
[Route("permissions")]
[Tags("Permissions")]
public class PermissionsController(IPermissionService permissionService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all permissions")]
    [EndpointDescription("Required permission: permission:read")]
    public ActionResult<List<PermissionResponse>> GetAllPermissions() => Ok(permissionService.GetAllPermissions());
}
