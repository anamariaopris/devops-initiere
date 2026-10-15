using OnlineSchoolApi.Courses.Models;
using OnlineSchoolApi.Users.Students.Models;

namespace OnlineSchoolApi.Enrolments.Models;

public class Enrolment
{
    public long Id { get; set; }
    public int? StudentId { get; set; }
    public Student? Student { get; set; }
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
}
