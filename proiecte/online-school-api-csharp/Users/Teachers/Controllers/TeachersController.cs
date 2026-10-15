using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Users.Teachers.Dtos;
using OnlineSchoolApi.Users.Teachers.Services;

namespace OnlineSchoolApi.Users.Teachers.Controllers;

[ApiController]
[Route("teachers")]
[Tags("Teachers")]
public class TeachersController(ITeacherQueryService teacherQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all teachers")]
    [EndpointDescription("Required permission: teacher:read")]
    public async Task<ActionResult<List<TeacherResponse>>> GetAllTeachers(CancellationToken cancellationToken)
        => Ok(await teacherQueryService.GetAllTeachersAsync(cancellationToken));

    [HttpGet("{teacherId}")]
    [EndpointSummary("Get teacher by id")]
    [EndpointDescription("Required permission: teacher:read")]
    public async Task<ActionResult<TeacherResponse>> GetTeacherById([FromRoute, Positive] long teacherId, CancellationToken cancellationToken)
        => Ok(await teacherQueryService.GetTeacherByIdAsync(teacherId, cancellationToken));

    [HttpGet("find-by-email")]
    [EndpointSummary("Find teacher by email")]
    [EndpointDescription("Required permission: teacher:read")]
    public async Task<ActionResult<TeacherResponse>> FindByEmail([FromQuery, BindRequired] string? email, CancellationToken cancellationToken)
        => Ok(await teacherQueryService.FindByEmailAsync(ParameterGuard.NotBlank(email), cancellationToken));

    [HttpGet("search")]
    [EndpointSummary("Search teachers")]
    [EndpointDescription("Required permission: teacher:read")]
    public async Task<ActionResult<List<TeacherResponse>>> SearchTeachers([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken)
        => Ok(await teacherQueryService.SearchTeachersAsync(query, cancellationToken));
}
