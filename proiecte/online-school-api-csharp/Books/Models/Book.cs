using System.ComponentModel.DataAnnotations;
using OnlineSchoolApi.Users.Students.Models;

namespace OnlineSchoolApi.Books.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string BookName { get; set; } = string.Empty;

    public int? StudentId { get; set; }

    public Student? Student { get; set; }
}
