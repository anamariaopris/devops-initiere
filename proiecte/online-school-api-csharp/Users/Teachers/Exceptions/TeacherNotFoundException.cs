using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Users.Teachers.Exceptions;

public class TeacherNotFoundException : NotFoundException
{
    public TeacherNotFoundException(long teacherId) : base($"teacher with id {teacherId} not found")
    {
    }

    public TeacherNotFoundException(string email) : base($"teacher with email {email} not found")
    {
    }
}
