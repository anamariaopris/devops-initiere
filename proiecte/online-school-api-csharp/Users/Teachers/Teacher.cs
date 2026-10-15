namespace OnlineSchoolApi.Users.Teachers;

public class Teacher : User
{
    public string? EmployeeCode { get; set; }
    public string? Specialization { get; set; }
    public string? Title { get; set; }
    public string? Bio { get; set; }
}
