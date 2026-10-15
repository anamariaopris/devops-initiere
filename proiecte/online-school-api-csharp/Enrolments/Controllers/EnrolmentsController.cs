using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Enrolments.Dtos;
using OnlineSchoolApi.Enrolments.Services;

namespace OnlineSchoolApi.Enrolments.Controllers;

[ApiController]
[Route("enrolments")]
[Tags("Enrolments")]
public class EnrolmentsController(IEnrolmentCommandService enrolmentCommandService, IEnrolmentQueryService enrolmentQueryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create enrolment")]
    [EndpointDescription("Required permission: enrolment:write")]
    [ProducesResponseType(typeof(EnrolmentResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<EnrolmentResponse>> CreateEnrolment([FromBody] EnrolmentCreateRequest request, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await enrolmentCommandService.EnrollStudentAsync(request, cancellationToken));

    [HttpGet]
    [EndpointSummary("Get all enrolments")]
    [EndpointDescription("Required permission: enrolment:read")]
    public async Task<ActionResult<List<EnrolmentResponse>>> GetAllEnrolments(CancellationToken cancellationToken)
        => Ok(await enrolmentQueryService.GetAllEnrolmentsAsync(cancellationToken));

    [HttpGet("{enrolmentId}")]
    [EndpointSummary("Get enrolment by id")]
    [EndpointDescription("Required permission: enrolment:read")]
    public async Task<ActionResult<EnrolmentResponse>> GetEnrolmentById([FromRoute, Positive] long enrolmentId, CancellationToken cancellationToken)
        => Ok(await enrolmentQueryService.GetEnrolmentByIdAsync(enrolmentId, cancellationToken));

    [HttpGet("find-by-student")]
    [EndpointSummary("Find enrolments by student")]
    [EndpointDescription("Required permission: enrolment:read")]
    public async Task<ActionResult<List<EnrolmentResponse>>> FindByStudent([FromQuery, BindRequired, Positive] long studentId, CancellationToken cancellationToken)
        => Ok(await enrolmentQueryService.FindByStudentIdAsync(studentId, cancellationToken));

    [HttpGet("find-by-course")]
    [EndpointSummary("Find enrolments by course")]
    [EndpointDescription("Required permission: enrolment:read")]
    public async Task<ActionResult<List<EnrolmentResponse>>> FindByCourse([FromQuery, BindRequired, Positive] long courseId, CancellationToken cancellationToken)
        => Ok(await enrolmentQueryService.FindByCourseIdAsync(courseId, cancellationToken));

    [HttpGet("search")]
    [EndpointSummary("Search enrolments")]
    [EndpointDescription("Required permission: enrolment:read")]
    public async Task<ActionResult<List<EnrolmentResponse>>> SearchEnrolments([FromQuery] long? studentId, [FromQuery] long? courseId, CancellationToken cancellationToken)
        => Ok(await enrolmentQueryService.SearchEnrolmentsAsync(studentId, courseId, cancellationToken));

    [HttpPut("{enrolmentId}")]
    [EndpointSummary("Update enrolment")]
    [EndpointDescription("Required permission: enrolment:write")]
    public async Task<ActionResult<EnrolmentResponse>> UpdateEnrolment([FromRoute, Positive] long enrolmentId, [FromBody] EnrolmentUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await enrolmentCommandService.UpdateEnrolmentAsync(enrolmentId, request, cancellationToken));

    [HttpPatch("{enrolmentId}")]
    [EndpointSummary("Patch enrolment")]
    [EndpointDescription("Required permission: enrolment:write")]
    public async Task<ActionResult<EnrolmentResponse>> PatchEnrolment([FromRoute, Positive] long enrolmentId, [FromBody] EnrolmentUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await enrolmentCommandService.UpdateEnrolmentAsync(enrolmentId, request, cancellationToken));

    [HttpDelete("{enrolmentId}")]
    [EndpointSummary("Delete enrolment")]
    [EndpointDescription("Required permission: enrolment:write")]
    public async Task<ActionResult<EnrolmentResponse>> DeleteEnrolment([FromRoute, Positive] long enrolmentId, CancellationToken cancellationToken)
        => Ok(await enrolmentCommandService.DeleteEnrolmentAsync(enrolmentId, cancellationToken));
}
