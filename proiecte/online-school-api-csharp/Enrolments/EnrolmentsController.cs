using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OnlineSchoolApi.Enrolments;

[ApiController]
[Route("enrolments")]
public class EnrolmentsController(EnrolmentService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "enrolment:read")]
    public Task<List<EnrolmentResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{enrolmentId}")]
    [Authorize(Policy = "enrolment:read")]
    public Task<EnrolmentResponse> GetById([Range(1, long.MaxValue)] long enrolmentId, CancellationToken cancellationToken) =>
        service.GetByIdAsync(enrolmentId, cancellationToken);

    [HttpGet("find-by-student")]
    [Authorize(Policy = "enrolment:read")]
    public Task<List<EnrolmentResponse>> FindByStudent([BindRequired, Range(1, long.MaxValue)] long studentId, CancellationToken cancellationToken) =>
        service.SearchAsync(studentId, null, cancellationToken);

    [HttpGet("find-by-course")]
    [Authorize(Policy = "enrolment:read")]
    public Task<List<EnrolmentResponse>> FindByCourse([BindRequired, Range(1, long.MaxValue)] long courseId, CancellationToken cancellationToken) =>
        service.SearchAsync(null, courseId, cancellationToken);

    [HttpGet("search")]
    [Authorize(Policy = "enrolment:read")]
    public Task<List<EnrolmentResponse>> Search(long? studentId, long? courseId, CancellationToken cancellationToken) =>
        service.SearchAsync(studentId, courseId, cancellationToken);

    [HttpPost]
    [Authorize(Policy = "enrolment:write")]
    public async Task<ActionResult<EnrolmentResponse>> Create(EnrolmentCreateRequest request, CancellationToken cancellationToken) =>
        StatusCode(StatusCodes.Status201Created, await service.CreateAsync(request, cancellationToken));

    [HttpPut("{enrolmentId}")]
    [Authorize(Policy = "enrolment:write")]
    public Task<EnrolmentResponse> Update([Range(1, long.MaxValue)] long enrolmentId, EnrolmentUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(enrolmentId, request, cancellationToken);

    [HttpPatch("{enrolmentId}")]
    [Authorize(Policy = "enrolment:write")]
    public Task<EnrolmentResponse> Patch([Range(1, long.MaxValue)] long enrolmentId, EnrolmentUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(enrolmentId, request, cancellationToken);

    [HttpDelete("{enrolmentId}")]
    [Authorize(Policy = "enrolment:write")]
    public Task<EnrolmentResponse> Delete([Range(1, long.MaxValue)] long enrolmentId, CancellationToken cancellationToken) =>
        service.DeleteAsync(enrolmentId, cancellationToken);
}
