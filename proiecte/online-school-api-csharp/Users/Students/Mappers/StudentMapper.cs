using OnlineSchoolApi.Users.Students.Dtos;
using OnlineSchoolApi.Users.Students.Models;

namespace OnlineSchoolApi.Users.Students.Mappers;

public static class StudentMapper
{
    public static Student ToEntity(StudentCreateRequest request) => new()
    {
        FirstName = request.FirstName ?? string.Empty,
        LastName = request.LastName ?? string.Empty,
        Email = request.Email ?? string.Empty
    };

    public static StudentResponse ToResponse(Student student) => new(
        student.Id,
        student.FirstName,
        student.LastName,
        student.Email
    );
}
