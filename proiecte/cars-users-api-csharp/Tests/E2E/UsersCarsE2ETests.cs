using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OneToManyApi.Cars;
using OneToManyApi.Users;

namespace OneToManyApi.Tests.E2E;

[Trait("Category", "E2E")]
public class UsersCarsE2ETests
{
    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:8103")
    };

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
    public async Task UserWithCar_CreateListDelete_WorksOverHttp()
    {
        // Arrange
        var userRequest = NewUser();

        // Act
        var createdUser = await client.PostAsJsonAsync("/api/users", userRequest);
        var user = await createdUser.Content.ReadFromJsonAsync<UserResponse>();
        var createdCar = await client.PostAsJsonAsync("/api/cars", new CarRequest("E2E", "Model", 2021, user!.Id));
        var car = await createdCar.Content.ReadFromJsonAsync<CarResponse>();
        var userCars = await client.GetFromJsonAsync<UserCarsResponse>($"/api/users/{user.Id}/cars");
        var deleted = await client.DeleteAsync($"/api/users/{user.Id}");
        var afterDelete = await client.GetAsync($"/api/users/{user.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, createdUser.StatusCode);
        Assert.Equal(HttpStatusCode.Created, createdCar.StatusCode);
        Assert.Contains(userCars!.Cars, c => c.Id == car!.Id);
        Assert.Equal(HttpStatusCode.OK, deleted.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, afterDelete.StatusCode);
    }

    [E2EFact]
    public async Task DuplicateEmail_ReturnsConflictProblemDetails()
    {
        // Arrange
        var userRequest = NewUser();
        await client.PostAsJsonAsync("/api/users", userRequest);

        // Act
        var response = await client.PostAsJsonAsync("/api/users", userRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(409, problem!.Status);
    }

    private static UserRequest NewUser() => new("E2E User", $"{Guid.NewGuid():N}@example.com");
}
