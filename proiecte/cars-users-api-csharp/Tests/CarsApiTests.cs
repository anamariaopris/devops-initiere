using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OneToManyApi.Cars;
using OneToManyApi.Users;

namespace OneToManyApi.Tests;

public class CarsApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task Create_ValidCar_Returns201WithLocation()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var request = NewCar(userId);

        // Act
        var response = await client.PostAsJsonAsync("/api/cars", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.NotNull(created);
        Assert.Equal(userId, created.UserId);
        Assert.Equal($"/api/cars/{created.Id}", response.Headers.Location?.AbsolutePath);
    }

    [Fact]
    public async Task Create_UnknownUser_Returns404Problem()
    {
        // Act
        var response = await client.PostAsJsonAsync("/api/cars", NewCar(999999999));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("User not found with id 999999999", problem.Detail);
    }

    [Fact]
    public async Task Create_InvalidYear_Returns400()
    {
        // Arrange
        var userId = await CreateUserAsync();

        // Act
        var response = await client.PostAsJsonAsync("/api/cars", NewCar(userId) with { Year = 1800 });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.True(problem.Errors.ContainsKey("Year"));
    }

    [Fact]
    public async Task Update_MoveToUnknownUser_Returns404()
    {
        // Arrange
        var created = await CreateCarAsync(await CreateUserAsync());

        // Act
        var response = await client.PutAsJsonAsync($"/api/cars/{created.Id}", NewCar(999999999));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_MoveToAnotherUser_Returns200()
    {
        // Arrange
        var created = await CreateCarAsync(await CreateUserAsync());
        var otherUserId = await CreateUserAsync();

        // Act
        var response = await client.PutAsJsonAsync($"/api/cars/{created.Id}", NewCar(otherUserId) with { Year = 2023 });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.NotNull(updated);
        Assert.Equal(otherUserId, updated.UserId);
        Assert.Equal(2023, updated.Year);
    }

    [Fact]
    public async Task Patch_EmptyBody_Returns400()
    {
        // Arrange
        var created = await CreateCarAsync(await CreateUserAsync());

        // Act
        var response = await client.PatchAsJsonAsync($"/api/cars/{created.Id}", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Patch request must contain at least one field", problem.Detail);
    }

    [Fact]
    public async Task Patch_YearOnly_KeepsOtherFields()
    {
        // Arrange
        var created = await CreateCarAsync(await CreateUserAsync());

        // Act
        var response = await client.PatchAsJsonAsync($"/api/cars/{created.Id}", new { year = 2024 });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.NotNull(updated);
        Assert.Equal(2024, updated.Year);
        Assert.Equal(created.Brand, updated.Brand);
    }

    [Fact]
    public async Task Patch_UnknownUser_Returns404()
    {
        // Arrange
        var created = await CreateCarAsync(await CreateUserAsync());

        // Act
        var response = await client.PatchAsJsonAsync($"/api/cars/{created.Id}", new { userId = 999999999 });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingCar_Returns200WithBody()
    {
        // Arrange
        var created = await CreateCarAsync(await CreateUserAsync());

        // Act
        var response = await client.DeleteAsync($"/api/cars/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var deleted = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.NotNull(deleted);
        Assert.Equal(created.Id, deleted.Id);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/cars/{created.Id}")).StatusCode);
    }

    [Fact]
    public async Task GetByYearRange_MinGreaterThanMax_Returns400Problem()
    {
        // Act
        var response = await client.GetAsync("/api/cars/by-year-range?minYear=2022&maxYear=2020");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Invalid year range: minYear (2022) must be <= maxYear (2020)", problem.Detail);
    }

    [Fact]
    public async Task GetByYearRange_NoMatch_Returns200WithEmptyWrapper()
    {
        // Act
        var response = await client.GetAsync("/api/cars/by-year-range?minYear=3000&maxYear=3001");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<CarListResponse>();
        Assert.NotNull(body);
        Assert.Empty(body.Cars);
    }

    [Fact]
    public async Task Search_ByUserId_ReturnsOnlyThatUsersCars()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var created = await CreateCarAsync(userId);

        // Act
        var response = await client.GetAsync($"/api/cars/search?userId={userId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<CarListResponse>();
        Assert.NotNull(body);
        Assert.Equal([created.Id], body.Cars.Select(c => c.Id));
    }

    [Fact]
    public async Task GetByBrand_UnknownBrand_Returns200WithEmptyWrapper()
    {
        // Act
        var response = await client.GetAsync($"/api/cars/by-brand/{Guid.NewGuid():N}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<CarListResponse>();
        Assert.NotNull(body);
        Assert.Empty(body.Cars);
    }

    private static CarRequest NewCar(long userId) => new($"Brand-{Guid.NewGuid():N}", "Model", 2020, userId);

    private async Task<long> CreateUserAsync()
    {
        var response = await client.PostAsJsonAsync("/api/users", new UserRequest("Owner", $"{Guid.NewGuid():N}@example.com"));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<UserResponse>())!.Id;
    }

    private async Task<CarResponse> CreateCarAsync(long userId)
    {
        var response = await client.PostAsJsonAsync("/api/cars", NewCar(userId));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CarResponse>())!;
    }
}
