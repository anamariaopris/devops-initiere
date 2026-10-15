using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OneModelApi.Dtos;

namespace OneModelApi.Tests.E2E;

[Trait("Category", "E2E")]
public class CarsE2ETests
{
    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:8102")
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
    public async Task CarLifecycle_CreateReadUpdateDelete_WorksOverHttp()
    {
        // Arrange
        var request = new CarRequest(Guid.NewGuid().ToString("N"), "E2E", "white", 5, 2021, 9500.50m, 42000, "petrol", "manual", true);

        // Act
        var created = await client.PostAsJsonAsync("/api/cars", request);
        var car = await created.Content.ReadFromJsonAsync<CarResponse>();
        var fetched = await client.GetFromJsonAsync<CarResponse>($"/api/cars/{car!.Id}");
        var updated = await client.PutAsJsonAsync($"/api/cars/{car.Id}", request with { Color = "blue" });
        var deleted = await client.DeleteAsync($"/api/cars/{car.Id}");
        var afterDelete = await client.GetAsync($"/api/cars/{car.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.Equal(9500.50m, fetched!.Price);
        Assert.Equal(HttpStatusCode.OK, updated.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, afterDelete.StatusCode);
    }

    [E2EFact]
    public async Task MissingCar_ReturnsProblemDetails()
    {
        // Act
        var response = await client.GetAsync("/api/cars/999999999");
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(404, problem!.Status);
    }
}
