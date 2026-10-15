using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OneModelApi.Dtos;

namespace OneModelApi.Tests;

public class CarsApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
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
    public async Task Create_ValidCar_Returns201WithLocation()
    {
        // Arrange
        var request = NewCar();

        // Act
        var response = await client.PostAsJsonAsync("/api/cars", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.NotNull(created);
        Assert.Equal($"/api/cars/{created.Id}", response.Headers.Location?.AbsolutePath);
    }

    [Fact]
    public async Task GetById_ExistingCar_Returns200WithFields()
    {
        // Arrange
        var request = NewCar();
        var created = await CreateAsync(request);

        // Act
        var response = await client.GetAsync($"/api/cars/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var car = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.Equal(new CarResponse(created.Id, request.Brand, request.Model, "white", 5, 2021, 9500.50m, 42000, "petrol", "manual", true), car);
    }

    [Fact]
    public async Task GetAll_WithCars_Returns200ContainingCreatedCar()
    {
        // Arrange
        var created = await CreateAsync(NewCar());

        // Act
        var response = await client.GetAsync("/api/cars");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var list = await response.Content.ReadFromJsonAsync<CarListResponse>();
        Assert.NotNull(list);
        Assert.Contains(list.Cars, c => c.Id == created.Id);
    }

    [Fact]
    public async Task GetByBrand_UnknownBrand_Returns200WithEmptyList()
    {
        // Act
        var response = await client.GetAsync($"/api/cars/brand/{Guid.NewGuid():N}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var list = await response.Content.ReadFromJsonAsync<CarListResponse>();
        Assert.NotNull(list);
        Assert.Empty(list.Cars);
    }

    [Fact]
    public async Task GetByBrand_ExistingBrand_Returns200WithOneCar()
    {
        // Arrange
        var created = await CreateAsync(NewCar());

        // Act
        var response = await client.GetAsync($"/api/cars/brand/{created.Brand}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var list = await response.Content.ReadFromJsonAsync<CarListResponse>();
        Assert.NotNull(list);
        Assert.Equal(created, Assert.Single(list.Cars));
    }

    [Fact]
    public async Task GetById_MissingCar_Returns404ProblemDetails()
    {
        // Act
        var response = await client.GetAsync("/api/cars/999999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
        Assert.Equal("Car not found with id 999999999", problem.Detail);
    }

    [Fact]
    public async Task Create_InvalidCar_Returns400ValidationProblem()
    {
        // Arrange
        var request = NewCar() with { Brand = "", Year = 1800 };

        // Act
        var response = await client.PostAsJsonAsync("/api/cars", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.True(problem.Errors.ContainsKey("Brand"));
        Assert.True(problem.Errors.ContainsKey("Year"));
    }

    [Fact]
    public async Task Create_DuplicateBrandAndModel_Returns409ProblemDetails()
    {
        // Arrange
        var request = NewCar();
        await CreateAsync(request);

        // Act
        var response = await client.PostAsJsonAsync("/api/cars", request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(409, problem.Status);
    }

    [Fact]
    public async Task Create_SameCarConcurrently_ReturnsOne201AndOnly409s()
    {
        // Arrange
        var request = NewCar();

        // Act
        var responses = await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => client.PostAsJsonAsync("/api/cars", request)));
        var statuses = responses.Select(r => r.StatusCode).ToList();

        // Assert
        Assert.Single(statuses, HttpStatusCode.Created);
        Assert.Equal(9, statuses.Count(s => s == HttpStatusCode.Conflict));
    }

    [Fact]
    public async Task Update_ExistingCar_Returns200WithNewValues()
    {
        // Arrange
        var created = await CreateAsync(NewCar());
        var request = NewCar() with { Brand = created.Brand, Model = created.Model, Color = "blue", Price = 10500.99m };

        // Act
        var response = await client.PutAsJsonAsync($"/api/cars/{created.Id}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var car = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.NotNull(car);
        Assert.Equal("blue", car.Color);
        Assert.Equal(10500.99m, car.Price);
    }

    [Fact]
    public async Task Patch_OneField_Returns200AndKeepsOtherFields()
    {
        // Arrange
        var created = await CreateAsync(NewCar());

        // Act
        var response = await client.PatchAsJsonAsync($"/api/cars/{created.Id}", new { available = false });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var car = await response.Content.ReadFromJsonAsync<CarResponse>();
        Assert.Equal(created with { Available = false }, car);
    }

    [Fact]
    public async Task Patch_EmptyBody_Returns400ProblemDetails()
    {
        // Arrange
        var created = await CreateAsync(NewCar());

        // Act
        var response = await client.PatchAsJsonAsync($"/api/cars/{created.Id}", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Patch request must contain at least one field", problem.Detail);
    }

    [Fact]
    public async Task Delete_ExistingCar_Returns204AndCarIsGone()
    {
        // Arrange
        var created = await CreateAsync(NewCar());

        // Act
        var response = await client.DeleteAsync($"/api/cars/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var after = await client.GetAsync($"/api/cars/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, after.StatusCode);
    }

    [Fact]
    public async Task Search_ByBrandAndMaxPrice_Returns200WithCar()
    {
        // Arrange
        var created = await CreateAsync(NewCar());

        // Act
        var response = await client.GetAsync($"/api/cars/search?brand={created.Brand}&maxPrice=9500.50&minYear=2020");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var list = await response.Content.ReadFromJsonAsync<CarListResponse>();
        Assert.NotNull(list);
        Assert.Equal(created, Assert.Single(list.Cars));
    }

    [Fact]
    public async Task Search_MinYearGreaterThanMaxYear_Returns400ProblemDetails()
    {
        // Act
        var response = await client.GetAsync("/api/cars/search?minYear=2020&maxYear=2010");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
    }

    private static CarRequest NewCar() => new(
        Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), "white", 5, 2021,
        9500.50m, 42000, "petrol", "manual", true);

    private async Task<CarResponse> CreateAsync(CarRequest request)
    {
        var response = await client.PostAsJsonAsync("/api/cars", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CarResponse>())!;
    }
}
