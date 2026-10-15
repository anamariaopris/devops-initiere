using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Courses.Dtos;
using OnlineSchoolApi.Courses.Exceptions;
using OnlineSchoolApi.Courses.Mappers;
using OnlineSchoolApi.Courses.Repositories;

namespace OnlineSchoolApi.Courses.Services;

public class CourseCommandService(ICourseRepository courseRepository) : ICourseCommandService
{
    public async Task<CourseResponse> CreateCourseAsync(CourseCreateRequest? request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new RequestBodyMissingException();
        }

        var course = CourseMapper.ToEntity(request);
        await courseRepository.AddAsync(course, cancellationToken);
        await courseRepository.SaveChangesAsync(cancellationToken);
        return CourseMapper.ToResponse(course);
    }

    public async Task<CourseResponse> UpdateCourseAsync(long courseId, CourseUpdateRequest? request, CancellationToken cancellationToken = default)
    {
        var course = await courseRepository.GetTrackedByIdAsync(courseId, cancellationToken)
            ?? throw new CourseNotFoundException(courseId);

        if (request is null)
        {
            throw new RequestBodyMissingException();
        }

        if (request.CourseName is null && request.Department is null)
        {
            throw new EmptyUpdateRequestException();
        }

        if (request.CourseName is not null)
        {
            course.CourseName = request.CourseName;
        }

        if (request.Department is not null)
        {
            course.Department = request.Department;
        }

        await courseRepository.SaveChangesAsync(cancellationToken);
        return CourseMapper.ToResponse(course);
    }

    public async Task<CourseResponse> DeleteCourseAsync(long courseId, CancellationToken cancellationToken = default)
    {
        var course = await courseRepository.GetTrackedByIdAsync(courseId, cancellationToken)
            ?? throw new CourseNotFoundException(courseId);

        var response = CourseMapper.ToResponse(course);
        courseRepository.Remove(course);
        await courseRepository.SaveChangesAsync(cancellationToken);
        return response;
    }
}
