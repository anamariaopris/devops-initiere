using System.Net;
using System.Net.Http.Json;
using mock_car_source_api.Models;

namespace mock_car_source_api.Tests;

public class ExternalCarsReadTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task HealthLive_Returns200()
    {
        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HealthReady_Returns200()
    {
        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_Returns200With120Cars()
    {
        // Act
        var response = await client.GetAsync("/api/external/cars");
        var cars = await response.Content.ReadFromJsonAsync<List<ExternalCarDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(120, cars!.Count);
    }

    [Fact]
    public async Task GetById_ExistingCar_Returns200()
    {
        // Arrange
        var cars = await client.GetFromJsonAsync<List<ExternalCarDto>>("/api/external/cars");
        var expected = cars![0];

        // Act
        var response = await client.GetAsync($"/api/external/cars/{expected.ExternalId}");
        var car = await response.Content.ReadFromJsonAsync<ExternalCarDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected.ExternalId, car!.ExternalId);
    }

    [Fact]
    public async Task GetById_UnknownId_Returns404Problem()
    {
        // Act
        var response = await client.GetAsync("/api/external/cars/unknown-id");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
    }

    [Fact]
    public async Task Regenerate_CountZero_Returns400()
    {
        // Act
        var response = await client.PostAsync("/api/external/cars/regenerate?count=0", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Regenerate_CountAboveMax_Returns400()
    {
        // Act
        var response = await client.PostAsync("/api/external/cars/regenerate?count=1001", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

public class ExternalCarsRegenerateTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task Regenerate_Count10_ReplacesCatalog()
    {
        // Arrange
        var before = await client.GetFromJsonAsync<List<ExternalCarDto>>("/api/external/cars");
        var oldId = before![0].ExternalId;

        // Act
        var response = await client.PostAsync("/api/external/cars/regenerate?count=10", null);
        var after = await client.GetFromJsonAsync<List<ExternalCarDto>>("/api/external/cars");
        var oldCar = await client.GetAsync($"/api/external/cars/{oldId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(10, after!.Count);
        Assert.Equal(HttpStatusCode.NotFound, oldCar.StatusCode);
    }
}
