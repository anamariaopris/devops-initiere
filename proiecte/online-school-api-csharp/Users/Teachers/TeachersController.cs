using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineSchoolApi.Users.Teachers;

[ApiController]
[Route("teachers")]
public class TeachersController(TeacherService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "teacher:read")]
    public Task<List<TeacherResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{teacherId}")]
    [Authorize(Policy = "teacher:read")]
    public Task<TeacherResponse> GetById([Range(1, long.MaxValue)] long teacherId, CancellationToken cancellationToken) =>
        service.GetByIdAsync(teacherId, cancellationToken);

    [HttpGet("find-by-email")]
    [Authorize(Policy = "teacher:read")]
    public Task<TeacherResponse> FindByEmail([Required] string email, CancellationToken cancellationToken) =>
        service.FindByEmailAsync(email, cancellationToken);

    [HttpGet("search")]
    [Authorize(Policy = "teacher:read")]
    public Task<List<TeacherResponse>> Search([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken) =>
        service.SearchAsync(query, cancellationToken);
}
