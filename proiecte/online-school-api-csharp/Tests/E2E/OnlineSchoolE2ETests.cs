using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using OnlineSchoolApi.Auth;
using OnlineSchoolApi.Courses;
using OnlineSchoolApi.Enrolments;
using OnlineSchoolApi.Users.Students;

namespace OnlineSchoolApi.Tests.E2E;

[Trait("Category", "E2E")]
public class OnlineSchoolE2ETests
{
    private static readonly Uri BaseUrl = new(Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:8110");

    private readonly HttpClient client = new() { BaseAddress = BaseUrl };

    private static async Task<HttpClient> LoginAsync(string email, string password)
    {
        var anonymous = new HttpClient { BaseAddress = BaseUrl };
        var response = await anonymous.PostAsJsonAsync("/auth/login", new AuthRequest(email, password));
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        var authenticated = new HttpClient { BaseAddress = BaseUrl };
        authenticated.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.AccessToken);
        return authenticated;
    }

    [E2EFact]
    public async Task HealthReady_ReturnsHealthy()
    {
        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

    [E2EFact]
    public async Task Swagger_IsServed()
    {
        // Act
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [E2EFact]
    public async Task Students_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await client.GetAsync("/students");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [E2EFact]
    public async Task CreateStudent_AsStudent_ReturnsForbidden()
    {
        // Arrange
        var student = await LoginAsync("ana.marinescu@student.online-school.ro", "password");
        var request = new StudentCreateRequest("Forbidden", "Student", $"{Guid.NewGuid():N}@e2e.ro");

        // Act
        var response = await student.PostAsJsonAsync("/students", request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [E2EFact]
    public async Task Me_WithToken_ReturnsCurrentUser()
    {
        // Arrange
        var admin = await LoginAsync("admin@online-school.ro", "password");

        // Act
        var response = await admin.GetAsync("/auth/me");
        var me = await response.Content.ReadFromJsonAsync<UserMeResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("admin@online-school.ro", me!.Email);
    }

    [E2EFact]
    public async Task Enrolment_CreateTwice_SecondReturnsConflict()
    {
        // Arrange
        var admin = await LoginAsync("admin@online-school.ro", "password");
        var student = await admin.GetFromJsonAsync<StudentResponse>("/students/find-by-email?email=ana.marinescu@student.online-school.ro");
        var courseRequest = new CourseCreateRequest($"E2E {Guid.NewGuid():N}", "E2E");

        // Act
        var createdCourse = await admin.PostAsJsonAsync("/courses", courseRequest);
        var course = await createdCourse.Content.ReadFromJsonAsync<CourseResponse>();
        var enrolment = new EnrolmentCreateRequest(student!.Id, course!.Id);
        var first = await admin.PostAsJsonAsync("/enrolments", enrolment);
        var second = await admin.PostAsJsonAsync("/enrolments", enrolment);
        var deletedCourse = await admin.DeleteAsync($"/courses/{course.Id}");
        var remaining = await admin.GetFromJsonAsync<List<EnrolmentResponse>>($"/enrolments/find-by-course?courseId={course.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, createdCourse.StatusCode);
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        Assert.Equal(HttpStatusCode.OK, deletedCourse.StatusCode);
        Assert.Empty(remaining!);
    }
}
