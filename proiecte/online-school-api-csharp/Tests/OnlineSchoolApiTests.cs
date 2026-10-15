using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineSchoolApi.Auth;
using OnlineSchoolApi.Courses;

namespace OnlineSchoolApi.Tests;

public class OnlineSchoolApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private const string AdminEmail = "admin@online-school.ro";
    private const string StudentEmail = "ana.marinescu@student.online-school.ro";
    private const string SeedPassword = "password";

    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task HealthLive_WithoutToken_Returns200()
    {
        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HealthReady_WithoutToken_Returns200()
    {
        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_SeededAdmin_Returns200WithToken()
    {
        // Act
        var response = await client.PostAsJsonAsync("/auth/login", new AuthRequest(AdminEmail, SeedPassword));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrWhiteSpace(auth.AccessToken));
        Assert.Equal("Bearer", auth.TokenType);
        Assert.Equal(86400, auth.ExpiresInSeconds);
        Assert.Equal(AdminEmail, auth.User.Email);
        Assert.Contains("student:write", auth.User.Permissions);
    }

    [Fact]
    public async Task Login_WrongPassword_Returns401ProblemDetails()
    {
        // Act
        var response = await client.PostAsJsonAsync("/auth/login", new AuthRequest(AdminEmail, "wrong-password"));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Invalid email or password", problem.Detail);
    }

    [Fact]
    public async Task GetStudents_WithoutToken_Returns401ProblemDetails()
    {
        // Act
        var response = await client.GetAsync("/students");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task CreateStudent_AsStudent_Returns403ProblemDetails()
    {
        // Arrange
        var student = await LoginAsync(StudentEmail);

        // Act
        var response = await student.PostAsJsonAsync("/students", new { firstName = "Test", lastName = "Student", email = UniqueEmail() });

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetStudents_AsAdmin_Returns200WithSeededStudentShape()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.GetAsync("/students");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("[{\"id\":20,\"firstName\":\"Ana\",\"lastName\":\"Marinescu\",\"email\":\"ana.marinescu@student.online-school.ro\"}", json);
    }

    [Fact]
    public async Task GetCourse_Seeded_Returns200WithDepartamentField()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.GetAsync("/courses/100");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{\"id\":100,\"courseName\":\"Algebra Fundamentals\",\"departament\":\"Mathematics\"}", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task GetStudentCourses_SeededStudent_Returns200WithEnrolledCourses()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.GetAsync("/students/20/courses");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var courses = await response.Content.ReadFromJsonAsync<List<CourseResponse>>();
        Assert.NotNull(courses);
        Assert.Equal([100, 101], courses.Select(c => c.Id).ToList());
    }

    [Fact]
    public async Task CreateBook_NameTooShort_Returns400ValidationProblem()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.PostAsJsonAsync("/books", new { bookName = "ab" });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.True(problem.Errors.ContainsKey("BookName"));
    }

    [Fact]
    public async Task GetBook_Missing_Returns404ProblemDetails()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.GetAsync("/books/9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Book not found with id=9999", problem.Detail);
    }

    [Fact]
    public async Task CreateEnrolment_Duplicate_Returns409ProblemDetails()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.PostAsJsonAsync("/enrolments", new { studentId = 20, courseId = 100 });

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Enrolment already exists for studentId=20 and courseId=100", problem.Detail);
    }

    [Fact]
    public async Task FindCoursesByDepartment_WithoutParameter_Returns400()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.GetAsync("/courses/find-by-department");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task PatchStudent_EmptyBody_Returns400ProblemDetails()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);

        // Act
        var response = await admin.PatchAsJsonAsync("/students/21", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Update request must contain at least one field to update", problem.Detail);
    }

    [Fact]
    public async Task DeleteCourse_Existing_Returns200WithDeletedCourse()
    {
        // Arrange
        var admin = await LoginAsync(AdminEmail);
        var created = await admin.PostAsJsonAsync("/courses", new { courseName = "Docker Basics", departament = "DevOps" });
        var course = await created.Content.ReadFromJsonAsync<CourseResponse>();

        // Act
        var response = await admin.DeleteAsync($"/courses/{course!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(new CourseResponse(course.Id, "Docker Basics", "DevOps"), await response.Content.ReadFromJsonAsync<CourseResponse>());
    }

    [Fact]
    public async Task Register_NewEmail_Returns201WithStudentPermissions()
    {
        // Arrange
        var request = new RegisterRequest("Test", "Student", UniqueEmail(), "password123", null);

        // Act
        var response = await client.PostAsJsonAsync("/auth/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(auth);
        Assert.Equal(request.Email, auth.User.Email);
        Assert.Contains("book:write", auth.User.Permissions);
        Assert.DoesNotContain("student:write", auth.User.Permissions);
    }

    [Fact]
    public async Task Register_ExistingEmail_Returns409ProblemDetails()
    {
        // Arrange
        var request = new RegisterRequest("Test", "Student", StudentEmail, "password123", null);

        // Act
        var response = await client.PostAsJsonAsync("/auth/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Me_WithToken_Returns200WithCurrentUser()
    {
        // Arrange
        var student = await LoginAsync(StudentEmail);

        // Act
        var response = await student.GetAsync("/auth/me");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var me = await response.Content.ReadFromJsonAsync<UserMeResponse>();
        Assert.NotNull(me);
        Assert.Equal(20, me.Id);
        Assert.Equal(StudentEmail, me.Email);
        Assert.Contains("student:read", me.Permissions);
    }

    [Fact]
    public void Startup_WithoutJwtSecret_ThrowsOptionsValidationException()
    {
        // Arrange
        using var withoutSecret = factory.WithWebHostBuilder(builder => builder.UseSetting("Jwt:Secret", ""));

        // Act
        var exception = Record.Exception(() => withoutSecret.CreateClient());

        // Assert
        var validation = Assert.IsType<OptionsValidationException>(exception);
        Assert.Equal(typeof(JwtOptions), validation.OptionsType);
    }

    private static string UniqueEmail() => $"test.{Guid.NewGuid():N}"[..20] + "@example.com";

    private async Task<HttpClient> LoginAsync(string email)
    {
        var response = await client.PostAsJsonAsync("/auth/login", new AuthRequest(email, SeedPassword));
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        var authorized = factory.CreateClient();
        authorized.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.AccessToken);
        return authorized;
    }
}
