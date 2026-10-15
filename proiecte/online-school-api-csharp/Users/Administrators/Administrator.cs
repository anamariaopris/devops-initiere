namespace OnlineSchoolApi.Users.Administrators;

public class Administrator : User
{
    public string? EmployeeCode { get; set; }
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
}
