using OnlineSchoolApi.Users.Students;

namespace OnlineSchoolApi.Books;

public class Book
{
    public int Id { get; set; }
    public string BookName { get; set; } = string.Empty;
    public int? StudentId { get; set; }
    public Student? Student { get; set; }
}
