using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using mock_car_source_api.Models;

namespace mock_car_source_api.Tests.E2E;

[Trait("Category", "E2E")]
public class ExternalCarsE2ETests
{
    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:8101")
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
    public async Task Catalog_ListAndReadByExternalId_WorksOverHttp()
    {
        // Act
        var cars = await client.GetFromJsonAsync<List<ExternalCarDto>>("/api/external/cars");
        var response = await client.GetAsync($"/api/external/cars/{cars![0].ExternalId}");
        var car = await response.Content.ReadFromJsonAsync<ExternalCarDto>();

        // Assert
        Assert.Equal(120, cars.Count);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(cars[0].ExternalId, car!.ExternalId);
    }

    [E2EFact]
    public async Task MissingCar_ReturnsProblemDetails()
    {
        // Act
        var response = await client.GetAsync($"/api/external/cars/{Guid.NewGuid()}");
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(404, problem!.Status);
    }
}
