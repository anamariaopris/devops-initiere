using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Courses.Dtos;
using OnlineSchoolApi.Courses.Services;

namespace OnlineSchoolApi.Courses.Controllers;

[ApiController]
[Route("courses")]
[Tags("Courses")]
public class CoursesController(ICourseCommandService courseCommandService, ICourseQueryService courseQueryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create course")]
    [EndpointDescription("Required permission: course:write")]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<CourseResponse>> CreateCourse([FromBody] CourseCreateRequest request, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await courseCommandService.CreateCourseAsync(request, cancellationToken));

    [HttpGet]
    [EndpointSummary("Get all courses")]
    [EndpointDescription("Required permission: course:read")]
    public async Task<ActionResult<List<CourseResponse>>> GetAllCourses(CancellationToken cancellationToken)
        => Ok(await courseQueryService.GetAllCoursesAsync(cancellationToken));

    [HttpGet("{courseId}")]
    [EndpointSummary("Get course by id")]
    [EndpointDescription("Required permission: course:read")]
    public async Task<ActionResult<CourseResponse>> GetCourseById([FromRoute, Positive] long courseId, CancellationToken cancellationToken)
        => Ok(await courseQueryService.GetCourseByIdAsync(courseId, cancellationToken));

    [HttpGet("find-by-department")]
    [EndpointSummary("Find courses by department")]
    [EndpointDescription("Required permission: course:read")]
    public async Task<ActionResult<List<CourseResponse>>> FindByDepartment([FromQuery, BindRequired] string? department, CancellationToken cancellationToken)
        => Ok(await courseQueryService.FindByDepartmentAsync(ParameterGuard.NotBlank(department), cancellationToken));

    [HttpGet("search")]
    [EndpointSummary("Search courses")]
    [EndpointDescription("Required permission: course:read")]
    public async Task<ActionResult<List<CourseResponse>>> SearchCourses([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken)
        => Ok(await courseQueryService.SearchCoursesAsync(query, cancellationToken));

    [HttpPut("{courseId}")]
    [EndpointSummary("Update course")]
    [EndpointDescription("Required permission: course:write")]
    public async Task<ActionResult<CourseResponse>> UpdateCourse([FromRoute, Positive] long courseId, [FromBody] CourseUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await courseCommandService.UpdateCourseAsync(courseId, request, cancellationToken));

    [HttpPatch("{courseId}")]
    [EndpointSummary("Patch course")]
    [EndpointDescription("Required permission: course:write")]
    public async Task<ActionResult<CourseResponse>> PatchCourse([FromRoute, Positive] long courseId, [FromBody] CourseUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await courseCommandService.UpdateCourseAsync(courseId, request, cancellationToken));

    [HttpDelete("{courseId}")]
    [EndpointSummary("Delete course")]
    [EndpointDescription("Required permission: course:write")]
    public async Task<ActionResult<CourseResponse>> DeleteCourse([FromRoute, Positive] long courseId, CancellationToken cancellationToken)
        => Ok(await courseCommandService.DeleteCourseAsync(courseId, cancellationToken));
}
