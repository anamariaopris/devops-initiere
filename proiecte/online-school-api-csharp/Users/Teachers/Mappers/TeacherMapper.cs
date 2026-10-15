using OnlineSchoolApi.Users.Teachers.Dtos;
using OnlineSchoolApi.Users.Teachers.Models;

namespace OnlineSchoolApi.Users.Teachers.Mappers;

public static class TeacherMapper
{
    public static TeacherResponse ToResponse(Teacher teacher) => new(
        teacher.Id,
        teacher.FirstName,
        teacher.LastName,
        teacher.Email,
        teacher.EmployeeCode,
        teacher.Specialization,
        teacher.Title
    );
}
