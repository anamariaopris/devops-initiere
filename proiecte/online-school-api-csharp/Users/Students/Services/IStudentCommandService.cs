using OnlineSchoolApi.Users.Students.Dtos;

namespace OnlineSchoolApi.Users.Students.Services;

public interface IStudentCommandService
{
    Task<StudentResponse> CreateStudentAsync(StudentCreateRequest? request, CancellationToken cancellationToken = default);
    Task<StudentResponse> UpdateStudentAsync(long studentId, StudentUpdateRequest? request, CancellationToken cancellationToken = default);
    Task<StudentResponse> DeleteStudentAsync(long studentId, CancellationToken cancellationToken = default);
}
