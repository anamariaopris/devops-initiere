namespace OnlineSchoolApi.Users.Teachers;

public record TeacherResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? EmployeeCode,
    string? Specialization,
    string? Title);

public static class TeacherMappings
{
    public static TeacherResponse ToResponse(this Teacher teacher) => new(
        teacher.Id, teacher.FirstName, teacher.LastName, teacher.Email,
        teacher.EmployeeCode, teacher.Specialization, teacher.Title);
}
