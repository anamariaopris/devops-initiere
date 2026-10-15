using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OneToManyApi.Cars;
using OneToManyApi.Users;

namespace OneToManyApi.Tests;

public class UsersApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task HealthReady_DatabaseUp_Returns200()
    {
        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HealthLive_Returns200()
    {
        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_SeededDatabase_ReturnsSeedUsersWithCars()
    {
        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UserWithCarsListResponse>();
        Assert.NotNull(body);
        var john = Assert.Single(body.Users, u => u.Email == "john.doe@example.com");
        var jane = Assert.Single(body.Users, u => u.Email == "jane.smith@example.com");
        Assert.Equal(["Toyota", "Honda"], john.Cars.Select(c => c.Brand));
        Assert.Equal(["Ford"], jane.Cars.Select(c => c.Brand));
    }

    [Fact]
    public async Task Create_ValidUser_Returns201WithLocation()
    {
        // Arrange
        var request = NewUser();

        // Act
        var response = await client.PostAsJsonAsync("/api/users", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<UserResponse>();
        Assert.NotNull(created);
        Assert.Equal(request.Email, created.Email);
        Assert.Equal($"/api/users/{created.Id}", response.Headers.Location?.AbsolutePath);
    }

    [Fact]
    public async Task Create_DuplicateEmail_Returns409Problem()
    {
        // Arrange
        var created = await CreateUserAsync();

        // Act
        var response = await client.PostAsJsonAsync("/api/users", NewUser() with { Email = created.Email.ToUpperInvariant() });

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(409, problem.Status);
    }

    [Fact]
    public async Task Create_InvalidBody_Returns400ValidationProblem()
    {
        // Arrange
        var request = new UserRequest("", "not-an-email");

        // Act
        var response = await client.PostAsJsonAsync("/api/users", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.True(problem.Errors.ContainsKey("Name"));
        Assert.True(problem.Errors.ContainsKey("Email"));
    }

    [Fact]
    public async Task GetById_UnknownUser_Returns404Problem()
    {
        // Act
        var response = await client.GetAsync("/api/users/999999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("User not found with id 999999999", problem.Detail);
    }

    [Fact]
    public async Task Update_OwnEmailWithDifferentCase_Returns200()
    {
        // Arrange
        var created = await CreateUserAsync();

        // Act
        var response = await client.PutAsJsonAsync($"/api/users/{created.Id}", new UserRequest("Renamed", created.Email.ToUpperInvariant()));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<UserResponse>();
        Assert.NotNull(updated);
        Assert.Equal("Renamed", updated.Name);
    }

    [Fact]
    public async Task Update_EmailOfAnotherUser_Returns409()
    {
        // Arrange
        var first = await CreateUserAsync();
        var second = await CreateUserAsync();

        // Act
        var response = await client.PutAsJsonAsync($"/api/users/{second.Id}", new UserRequest(second.Name, first.Email));

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Patch_EmptyBody_Returns400()
    {
        // Arrange
        var created = await CreateUserAsync();

        // Act
        var response = await client.PatchAsJsonAsync($"/api/users/{created.Id}", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Patch request must contain at least one field", problem.Detail);
    }

    [Fact]
    public async Task Patch_NameOnly_KeepsEmail()
    {
        // Arrange
        var created = await CreateUserAsync();

        // Act
        var response = await client.PatchAsJsonAsync($"/api/users/{created.Id}", new { name = "Patched" });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<UserResponse>();
        Assert.NotNull(updated);
        Assert.Equal("Patched", updated.Name);
        Assert.Equal(created.Email, updated.Email);
    }

    [Fact]
    public async Task Delete_ExistingUser_Returns200WithBodyAndRemovesCars()
    {
        // Arrange
        var created = await CreateUserAsync();
        var car = await CreateCarAsync(created.Id);

        // Act
        var response = await client.DeleteAsync($"/api/users/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var deleted = await response.Content.ReadFromJsonAsync<UserResponse>();
        Assert.NotNull(deleted);
        Assert.Equal(created.Id, deleted.Id);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/users/{created.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/cars/{car.Id}")).StatusCode);
    }

    [Fact]
    public async Task GetCars_ExistingUser_ReturnsWrapperWithCars()
    {
        // Arrange
        var created = await CreateUserAsync();
        var car = await CreateCarAsync(created.Id);

        // Act
        var response = await client.GetAsync($"/api/users/{created.Id}/cars");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UserCarsResponse>();
        Assert.NotNull(body);
        Assert.Equal(created.Id, body.UserId);
        Assert.Equal([car.Id], body.Cars.Select(c => c.Id));
    }

    [Fact]
    public async Task GetCars_UnknownUser_Returns404()
    {
        // Act
        var response = await client.GetAsync("/api/users/999999999/cars");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCars_UserWithoutCars_Returns200WithEmptyList()
    {
        // Arrange
        var created = await CreateUserAsync();

        // Act
        var response = await client.GetAsync($"/api/users/{created.Id}/cars");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UserCarsResponse>();
        Assert.NotNull(body);
        Assert.Empty(body.Cars);
    }

    [Fact]
    public async Task Search_NoMatch_Returns200WithEmptyArray()
    {
        // Act
        var response = await client.GetAsync($"/api/users/search?name={Guid.NewGuid():N}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
        Assert.NotNull(users);
        Assert.Empty(users);
    }

    private static UserRequest NewUser() => new("Test User", $"{Guid.NewGuid():N}@example.com");

    private async Task<UserResponse> CreateUserAsync()
    {
        var response = await client.PostAsJsonAsync("/api/users", NewUser());
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<UserResponse>())!;
    }

    private async Task<CarResponse> CreateCarAsync(long userId)
    {
        var response = await client.PostAsJsonAsync("/api/cars", new CarRequest($"Brand-{Guid.NewGuid():N}", "Model", 2020, userId));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CarResponse>())!;
    }
}
