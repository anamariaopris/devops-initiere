using OnlineSchoolApi.Courses.Dtos;

namespace OnlineSchoolApi.Courses.Services;

public interface ICourseCommandService
{
    Task<CourseResponse> CreateCourseAsync(CourseCreateRequest? request, CancellationToken cancellationToken = default);
    Task<CourseResponse> UpdateCourseAsync(long courseId, CourseUpdateRequest? request, CancellationToken cancellationToken = default);
    Task<CourseResponse> DeleteCourseAsync(long courseId, CancellationToken cancellationToken = default);
}
