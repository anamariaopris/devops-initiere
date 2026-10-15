using OnlineSchoolApi.Books.Models;
using OnlineSchoolApi.Enrolments.Models;
using OnlineSchoolApi.Users.Models;

namespace OnlineSchoolApi.Users.Students.Models;

public class Student : User
{
    public string? StudentCode { get; set; }
    public DateOnly? EnrollmentDate { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? GuardianName { get; set; }
    public string? GuardianEmail { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
