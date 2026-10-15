using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Courses.Dtos;
using OnlineSchoolApi.Users.Students.Dtos;
using OnlineSchoolApi.Users.Students.Services;

namespace OnlineSchoolApi.Users.Students.Controllers;

[ApiController]
[Route("students")]
[Tags("Students")]
public class StudentsController(IStudentCommandService studentCommandService, IStudentQueryService studentQueryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create student")]
    [EndpointDescription("Required permission: student:write")]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<StudentResponse>> CreateStudent([FromBody] StudentCreateRequest request, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await studentCommandService.CreateStudentAsync(request, cancellationToken));

    [HttpGet]
    [EndpointSummary("Get all students")]
    [EndpointDescription("Required permission: student:read")]
    public async Task<ActionResult<List<StudentResponse>>> GetAllStudents(CancellationToken cancellationToken)
        => Ok(await studentQueryService.GetAllStudentsAsync(cancellationToken));

    [HttpGet("{studentId}")]
    [EndpointSummary("Get student by id")]
    [EndpointDescription("Required permission: student:read")]
    public async Task<ActionResult<StudentResponse>> GetStudentById([FromRoute, Positive] long studentId, CancellationToken cancellationToken)
        => Ok(await studentQueryService.GetStudentByIdAsync(studentId, cancellationToken));

    [HttpGet("find-by-email")]
    [EndpointSummary("Find student by email")]
    [EndpointDescription("Required permission: student:read")]
    public async Task<ActionResult<StudentResponse>> FindByEmail([FromQuery, BindRequired] string? email, CancellationToken cancellationToken)
        => Ok(await studentQueryService.FindByEmailAsync(ParameterGuard.NotBlank(email), cancellationToken));

    [HttpGet("search")]
    [EndpointSummary("Search students")]
    [EndpointDescription("Required permission: student:read")]
    public async Task<ActionResult<List<StudentResponse>>> SearchStudents([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken)
        => Ok(await studentQueryService.SearchStudentsAsync(query, cancellationToken));

    [HttpGet("{studentId}/courses")]
    [EndpointSummary("Get courses for student")]
    [EndpointDescription("Required permission: student:read")]
    public async Task<ActionResult<List<CourseResponse>>> GetCoursesForStudent([FromRoute, Positive] long studentId, CancellationToken cancellationToken)
        => Ok(await studentQueryService.GetCoursesForStudentAsync(studentId, cancellationToken));

    [HttpPut("{studentId}")]
    [EndpointSummary("Update student")]
    [EndpointDescription("Required permission: student:write")]
    public async Task<ActionResult<StudentResponse>> UpdateStudent([FromRoute, Positive] long studentId, [FromBody] StudentUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await studentCommandService.UpdateStudentAsync(studentId, request, cancellationToken));

    [HttpPatch("{studentId}")]
    [EndpointSummary("Patch student")]
    [EndpointDescription("Required permission: student:write")]
    public async Task<ActionResult<StudentResponse>> PatchStudent([FromRoute, Positive] long studentId, [FromBody] StudentUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await studentCommandService.UpdateStudentAsync(studentId, request, cancellationToken));

    [HttpDelete("{studentId}")]
    [EndpointSummary("Delete student")]
    [EndpointDescription("Required permission: student:write")]
    public async Task<ActionResult<StudentResponse>> DeleteStudent([FromRoute, Positive] long studentId, CancellationToken cancellationToken)
        => Ok(await studentCommandService.DeleteStudentAsync(studentId, cancellationToken));
}
