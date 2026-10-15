using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineSchoolApi.Courses;

[ApiController]
[Route("courses")]
public class CoursesController(CourseService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "course:read")]
    public Task<List<CourseResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{courseId}")]
    [Authorize(Policy = "course:read")]
    public Task<CourseResponse> GetById([Range(1, long.MaxValue)] long courseId, CancellationToken cancellationToken) =>
        service.GetByIdAsync(courseId, cancellationToken);

    [HttpGet("find-by-department")]
    [Authorize(Policy = "course:read")]
    public Task<List<CourseResponse>> FindByDepartment([Required] string department, CancellationToken cancellationToken) =>
        service.FindByDepartmentAsync(department, cancellationToken);

    [HttpGet("search")]
    [Authorize(Policy = "course:read")]
    public Task<List<CourseResponse>> Search([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken) =>
        service.SearchAsync(query, cancellationToken);

    [HttpPost]
    [Authorize(Policy = "course:write")]
    public async Task<ActionResult<CourseResponse>> Create(CourseCreateRequest request, CancellationToken cancellationToken)
    {
        var course = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { courseId = course.Id }, course);
    }

    [HttpPut("{courseId}")]
    [Authorize(Policy = "course:write")]
    public Task<CourseResponse> Update([Range(1, long.MaxValue)] long courseId, CourseUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(courseId, request, cancellationToken);

    [HttpPatch("{courseId}")]
    [Authorize(Policy = "course:write")]
    public Task<CourseResponse> Patch([Range(1, long.MaxValue)] long courseId, CourseUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(courseId, request, cancellationToken);

    [HttpDelete("{courseId}")]
    [Authorize(Policy = "course:write")]
    public Task<CourseResponse> Delete([Range(1, long.MaxValue)] long courseId, CancellationToken cancellationToken) =>
        service.DeleteAsync(courseId, cancellationToken);
}
