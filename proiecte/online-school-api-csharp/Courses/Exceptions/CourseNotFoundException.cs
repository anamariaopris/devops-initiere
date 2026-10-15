using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Courses.Exceptions;

public class CourseNotFoundException(long courseId) : NotFoundException($"Course not found with id={courseId}");
