using OnlineSchoolApi.Courses.Dtos;
using OnlineSchoolApi.Courses.Exceptions;
using OnlineSchoolApi.Courses.Mappers;
using OnlineSchoolApi.Courses.Repositories;

namespace OnlineSchoolApi.Courses.Services;

public class CourseQueryService(ICourseRepository courseRepository) : ICourseQueryService
{
    public async Task<List<CourseResponse>> GetAllCoursesAsync(CancellationToken cancellationToken = default)
        => (await courseRepository.GetAllAsync(cancellationToken)).Select(CourseMapper.ToResponse).ToList();

    public async Task<CourseResponse> GetCourseByIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        var course = await courseRepository.GetByIdAsync(courseId, cancellationToken)
            ?? throw new CourseNotFoundException(courseId);

        return CourseMapper.ToResponse(course);
    }

    public async Task<List<CourseResponse>> FindByDepartmentAsync(string department, CancellationToken cancellationToken = default)
        => (await courseRepository.GetByDepartmentContainingAsync(department, cancellationToken)).Select(CourseMapper.ToResponse).ToList();

    public async Task<List<CourseResponse>> SearchCoursesAsync(string? query, CancellationToken cancellationToken = default)
    {
        var normalized = query?.Trim() ?? string.Empty;
        if (normalized.Length == 0)
        {
            return [];
        }

        return (await courseRepository.SearchAsync(normalized, cancellationToken)).Select(CourseMapper.ToResponse).ToList();
    }
}
