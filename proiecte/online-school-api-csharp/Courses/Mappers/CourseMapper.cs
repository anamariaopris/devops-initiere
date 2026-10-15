using OnlineSchoolApi.Courses.Dtos;
using OnlineSchoolApi.Courses.Models;

namespace OnlineSchoolApi.Courses.Mappers;

public static class CourseMapper
{
    public static Course ToEntity(CourseCreateRequest request) => new()
    {
        CourseName = request.CourseName ?? string.Empty,
        Department = request.Departament ?? string.Empty
    };

    public static CourseResponse ToResponse(Course course) => new(course.Id, course.CourseName, course.Department);
}
