using OnlineSchoolApi.Courses;
using OnlineSchoolApi.Users.Students;

namespace OnlineSchoolApi.Enrolments;

public class Enrolment
{
    public long Id { get; set; }
    public int? StudentId { get; set; }
    public Student? Student { get; set; }
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
}
