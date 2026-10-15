using OnlineSchoolApi.Enrolments.Dtos;

namespace OnlineSchoolApi.Enrolments.Services;

public interface IEnrolmentQueryService
{
    Task<List<EnrolmentResponse>> GetAllEnrolmentsAsync(CancellationToken cancellationToken = default);
    Task<EnrolmentResponse> GetEnrolmentByIdAsync(long enrolmentId, CancellationToken cancellationToken = default);
    Task<List<EnrolmentResponse>> FindByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<List<EnrolmentResponse>> FindByCourseIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task<List<EnrolmentResponse>> SearchEnrolmentsAsync(long? studentId, long? courseId, CancellationToken cancellationToken = default);
}
