using OnlineSchoolApi.Courses.Dtos;

namespace OnlineSchoolApi.Courses.Services;

public interface ICourseQueryService
{
    Task<List<CourseResponse>> GetAllCoursesAsync(CancellationToken cancellationToken = default);
    Task<CourseResponse> GetCourseByIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task<List<CourseResponse>> FindByDepartmentAsync(string department, CancellationToken cancellationToken = default);
    Task<List<CourseResponse>> SearchCoursesAsync(string? query, CancellationToken cancellationToken = default);
}
