using OnlineSchoolApi.Users.Teachers.Dtos;

namespace OnlineSchoolApi.Users.Teachers.Services;

public interface ITeacherQueryService
{
    Task<List<TeacherResponse>> GetAllTeachersAsync(CancellationToken cancellationToken = default);
    Task<TeacherResponse> GetTeacherByIdAsync(long teacherId, CancellationToken cancellationToken = default);
    Task<TeacherResponse> FindByEmailAsync(string? email, CancellationToken cancellationToken = default);
    Task<List<TeacherResponse>> SearchTeachersAsync(string? query, CancellationToken cancellationToken = default);
}
