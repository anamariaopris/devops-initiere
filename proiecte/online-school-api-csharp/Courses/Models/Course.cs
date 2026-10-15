using System.ComponentModel.DataAnnotations;
using OnlineSchoolApi.Enrolments.Models;

namespace OnlineSchoolApi.Courses.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = string.Empty;

    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
