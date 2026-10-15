using OnlineSchoolApi.Courses.Dtos;
using OnlineSchoolApi.Users.Students.Dtos;

namespace OnlineSchoolApi.Users.Students.Services;

public interface IStudentQueryService
{
    Task<List<StudentResponse>> GetAllStudentsAsync(CancellationToken cancellationToken = default);
    Task<StudentResponse> GetStudentByIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<StudentResponse> FindByEmailAsync(string? email, CancellationToken cancellationToken = default);
    Task<List<StudentResponse>> SearchStudentsAsync(string? query, CancellationToken cancellationToken = default);
    Task<List<CourseResponse>> GetCoursesForStudentAsync(long studentId, CancellationToken cancellationToken = default);
}
