using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchoolApi.Courses;

namespace OnlineSchoolApi.Users.Students;

[ApiController]
[Route("students")]
public class StudentsController(StudentService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "student:read")]
    public Task<List<StudentResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{studentId}")]
    [Authorize(Policy = "student:read")]
    public Task<StudentResponse> GetById([Range(1, long.MaxValue)] long studentId, CancellationToken cancellationToken) =>
        service.GetByIdAsync(studentId, cancellationToken);

    [HttpGet("find-by-email")]
    [Authorize(Policy = "student:read")]
    public Task<StudentResponse> FindByEmail([Required] string email, CancellationToken cancellationToken) =>
        service.FindByEmailAsync(email, cancellationToken);

    [HttpGet("search")]
    [Authorize(Policy = "student:read")]
    public Task<List<StudentResponse>> Search([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken) =>
        service.SearchAsync(query, cancellationToken);

    [HttpGet("{studentId}/courses")]
    [Authorize(Policy = "student:read")]
    public Task<List<CourseResponse>> GetCourses([Range(1, long.MaxValue)] long studentId, CancellationToken cancellationToken) =>
        service.GetCoursesAsync(studentId, cancellationToken);

    [HttpPost]
    [Authorize(Policy = "student:write")]
    public async Task<ActionResult<StudentResponse>> Create(StudentCreateRequest request, CancellationToken cancellationToken)
    {
        var student = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { studentId = student.Id }, student);
    }

    [HttpPut("{studentId}")]
    [Authorize(Policy = "student:write")]
    public Task<StudentResponse> Update([Range(1, long.MaxValue)] long studentId, StudentUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(studentId, request, cancellationToken);

    [HttpPatch("{studentId}")]
    [Authorize(Policy = "student:write")]
    public Task<StudentResponse> Patch([Range(1, long.MaxValue)] long studentId, StudentUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(studentId, request, cancellationToken);

    [HttpDelete("{studentId}")]
    [Authorize(Policy = "student:write")]
    public Task<StudentResponse> Delete([Range(1, long.MaxValue)] long studentId, CancellationToken cancellationToken) =>
        service.DeleteAsync(studentId, cancellationToken);
}
