using OnlineSchoolApi.Books.Models;
using OnlineSchoolApi.Courses.Models;
using OnlineSchoolApi.Enrolments.Models;
using OnlineSchoolApi.Users.Administrators.Models;
using OnlineSchoolApi.Users.Models;
using OnlineSchoolApi.Users.Students.Models;
using OnlineSchoolApi.Users.Teachers.Models;

namespace OnlineSchoolApi.Data.Seed;

public static class InitialData
{
    private const string PasswordHash = "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi";
    private static readonly DateTime SeededAt = new(2026, 4, 3, 9, 0, 0);
    private static readonly DateOnly EnrollmentDate = new(2025, 9, 15);

    private static readonly int[] AdministratorIds = [1];
    private static readonly int[] TeacherIds = [10, 11, 12, 13, 14];
    private static readonly int[] StudentIds = [20, 21, 22, 23, 24];

    public static readonly Administrator[] Administrators =
    [
        new Administrator
        {
            Id = 1, FirstName = "System", LastName = "Administrator", Email = "admin@online-school.ro", PhoneNumber = "+40740000001",
            PasswordHash = PasswordHash, IsActive = true, CreatedAt = SeededAt, UpdatedAt = SeededAt,
            EmployeeCode = "ADM-001", Department = "Operations", JobTitle = "Platform Administrator"
        }
    ];

    public static readonly Teacher[] Teachers =
    [
        Teacher(10, "Andrei", "Ionescu", "andrei.ionescu@online-school.ro", "+40740000010", "TCH-001", "Mathematics", "Senior Mathematics Teacher", "Coordoneaza curricula de matematica pentru gimnaziu si liceu."),
        Teacher(11, "Maria", "Popescu", "maria.popescu@online-school.ro", "+40740000011", "TCH-002", "Computer Science", "Software Engineering Teacher", "Preda baze de programare, algoritmi si proiectare software."),
        Teacher(12, "Alexandru", "Stan", "alexandru.stan@online-school.ro", "+40740000012", "TCH-003", "Physics", "Applied Physics Teacher", "Acopera fizica experimentala si aplicatii practice in laborator."),
        Teacher(13, "Ioana", "Dumitrescu", "ioana.dumitrescu@online-school.ro", "+40740000013", "TCH-004", "English", "Academic English Teacher", "Lucreaza pe comunicare academica si pregatire pentru examene internationale."),
        Teacher(14, "Radu", "Georgescu", "radu.georgescu@online-school.ro", "+40740000014", "TCH-005", "History", "Modern History Teacher", "Preda istorie moderna si gandire critica bazata pe surse.")
    ];

    public static readonly Student[] Students =
    [
        Student(20, "Ana", "Marinescu", "ana.marinescu@student.online-school.ro", "+40740000020", "STD-001", new DateOnly(2008, 3, 12), "Mihai Marinescu", "mihai.marinescu@example.com"),
        Student(21, "Vlad", "Petrescu", "vlad.petrescu@student.online-school.ro", "+40740000021", "STD-002", new DateOnly(2007, 11, 2), "Alina Petrescu", "alina.petrescu@example.com"),
        Student(22, "Elena", "Rusu", "elena.rusu@student.online-school.ro", "+40740000022", "STD-003", new DateOnly(2008, 7, 24), "Cristian Rusu", "cristian.rusu@example.com"),
        Student(23, "Matei", "Enache", "matei.enache@student.online-school.ro", "+40740000023", "STD-004", new DateOnly(2007, 5, 19), "Gabriela Enache", "gabriela.enache@example.com"),
        Student(24, "Sofia", "Tudor", "sofia.tudor@student.online-school.ro", "+40740000024", "STD-005", new DateOnly(2008, 1, 30), "Daniel Tudor", "daniel.tudor@example.com")
    ];

    public static readonly UserPermission[] UserPermissions =
    [
        .. Grant(StudentIds, PermissionTemplates.StudentDefaults()),
        .. Grant(TeacherIds, PermissionTemplates.TeacherDefaults()),
        .. Grant(AdministratorIds, PermissionTemplates.AdministratorDefaults())
    ];

    public static readonly Course[] Courses =
    [
        new Course { Id = 100, CourseName = "Algebra Fundamentals", Department = "Mathematics" },
        new Course { Id = 101, CourseName = "Java Programming Basics", Department = "Computer Science" },
        new Course { Id = 102, CourseName = "Physics Lab Essentials", Department = "Science" },
        new Course { Id = 103, CourseName = "Academic Writing in English", Department = "Languages" },
        new Course { Id = 104, CourseName = "Modern European History", Department = "Humanities" },
        new Course { Id = 105, CourseName = "Data Structures", Department = "Computer Science" }
    ];

    public static readonly Book[] Books =
    [
        new Book { Id = 200, BookName = "Algebra Practice Workbook", StudentId = 20 },
        new Book { Id = 201, BookName = "Java for Beginners", StudentId = 21 },
        new Book { Id = 202, BookName = "Physics Experiments Handbook", StudentId = 22 },
        new Book { Id = 203, BookName = "Academic English Companion", StudentId = 23 },
        new Book { Id = 204, BookName = "History Sourcebook", StudentId = 24 },
        new Book { Id = 205, BookName = "Advanced Data Structures Notes", StudentId = 21 },
        new Book { Id = 206, BookName = "Problem Solving in Mathematics", StudentId = 20 }
    ];

    public static readonly Enrolment[] Enrolments =
    [
        new Enrolment { Id = 300, StudentId = 20, CourseId = 100 },
        new Enrolment { Id = 301, StudentId = 20, CourseId = 101 },
        new Enrolment { Id = 302, StudentId = 21, CourseId = 101 },
        new Enrolment { Id = 303, StudentId = 21, CourseId = 105 },
        new Enrolment { Id = 304, StudentId = 22, CourseId = 102 },
        new Enrolment { Id = 305, StudentId = 23, CourseId = 103 },
        new Enrolment { Id = 306, StudentId = 24, CourseId = 104 },
        new Enrolment { Id = 307, StudentId = 24, CourseId = 100 }
    ];

    private static Teacher Teacher(int id, string firstName, string lastName, string email, string phone,
        string employeeCode, string specialization, string title, string bio) => new()
    {
        Id = id, FirstName = firstName, LastName = lastName, Email = email, PhoneNumber = phone,
        PasswordHash = PasswordHash, IsActive = true, CreatedAt = SeededAt, UpdatedAt = SeededAt,
        EmployeeCode = employeeCode, Specialization = specialization, Title = title, Bio = bio
    };

    private static Student Student(int id, string firstName, string lastName, string email, string phone,
        string studentCode, DateOnly dateOfBirth, string guardianName, string guardianEmail) => new()
    {
        Id = id, FirstName = firstName, LastName = lastName, Email = email, PhoneNumber = phone,
        PasswordHash = PasswordHash, IsActive = true, CreatedAt = SeededAt, UpdatedAt = SeededAt,
        StudentCode = studentCode, EnrollmentDate = EnrollmentDate, DateOfBirth = dateOfBirth,
        GuardianName = guardianName, GuardianEmail = guardianEmail
    };

    private static IEnumerable<UserPermission> Grant(IEnumerable<int> userIds, IEnumerable<Permission> permissions)
        => userIds.SelectMany(_ => permissions, (userId, permission) => new UserPermission { UserId = userId, Permission = permission });
}
