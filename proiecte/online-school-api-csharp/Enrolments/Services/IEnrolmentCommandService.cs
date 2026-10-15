using OnlineSchoolApi.Enrolments.Dtos;

namespace OnlineSchoolApi.Enrolments.Services;

public interface IEnrolmentCommandService
{
    Task<EnrolmentResponse> EnrollStudentAsync(EnrolmentCreateRequest? request, CancellationToken cancellationToken = default);
    Task<EnrolmentResponse> UpdateEnrolmentAsync(long enrolmentId, EnrolmentUpdateRequest? request, CancellationToken cancellationToken = default);
    Task<EnrolmentResponse> DeleteEnrolmentAsync(long enrolmentId, CancellationToken cancellationToken = default);
}
