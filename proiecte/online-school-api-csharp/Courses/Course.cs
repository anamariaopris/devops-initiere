using OnlineSchoolApi.Enrolments;

namespace OnlineSchoolApi.Courses;

public class Course
{
    public int Id { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
