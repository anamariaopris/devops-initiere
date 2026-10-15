using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineShopApi.Dtos;

namespace OnlineShopApi.Tests;

public class OnlineShopApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
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
    public async Task HealthReady_DatabaseUp_Returns200()
    {
        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_SeededAdmin_Returns200WithAccessAndRefreshTokens()
    {
        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", new { email = ApiFactory.AdminEmail, password = ApiFactory.AdminPassword });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrEmpty(auth.AccessToken));
        Assert.False(string.IsNullOrEmpty(auth.RefreshToken));
        Assert.Equal(ApiFactory.AdminEmail, auth.Email);
        Assert.Contains("USER_READ", auth.Permissions);
    }

    [Fact]
    public async Task Login_WrongPassword_Returns401ProblemDetails()
    {
        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", new { email = ApiFactory.AdminEmail, password = "wrong-password" });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Invalid email or password.", problem.Detail);
    }

    [Fact]
    public async Task GetProducts_WithoutToken_Returns401ProblemDetails()
    {
        // Act
        var response = await client.GetAsync("/api/products");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Refresh_ValidRefreshToken_Returns200WithNewTokens()
    {
        // Arrange
        var login = await LoginAsync(ApiFactory.AdminEmail, ApiFactory.AdminPassword);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/refresh", new { refreshToken = login.RefreshToken });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(auth);
        Assert.Equal(ApiFactory.AdminEmail, auth.Email);
        Assert.False(string.IsNullOrEmpty(auth.AccessToken));
    }

    [Fact]
    public async Task GetProducts_RefreshTokenAsBearer_Returns401()
    {
        // Arrange
        var login = await LoginAsync(ApiFactory.AdminEmail, ApiFactory.AdminPassword);
        var request = Authorized(HttpMethod.Get, "/api/products", login.RefreshToken);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_AsCustomer_Returns403ProblemDetails()
    {
        // Arrange
        var login = await LoginAsync(ApiFactory.CustomerEmail, ApiFactory.CustomerPassword);
        var request = Authorized(HttpMethod.Get, "/api/users", login.AccessToken);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetCustomerById_Owner_Returns200()
    {
        // Arrange
        var login = await LoginAsync(ApiFactory.CustomerEmail, ApiFactory.CustomerPassword);
        var me = await GetMyCustomerAsync(login.AccessToken);
        var request = Authorized(HttpMethod.Get, $"/api/customers/{me.Id}", login.AccessToken);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var customer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        Assert.NotNull(customer);
        Assert.Equal(ApiFactory.CustomerEmail, customer.Email);
    }

    [Fact]
    public async Task GetCustomerById_OtherCustomer_Returns403()
    {
        // Arrange
        var other = await RegisterAsync(NewRegistration());
        var otherCustomer = await GetMyCustomerAsync(other.AccessToken);
        var login = await LoginAsync(ApiFactory.CustomerEmail, ApiFactory.CustomerPassword);
        var request = Authorized(HttpMethod.Get, $"/api/customers/{otherCustomer.Id}", login.AccessToken);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_AsAdminWithPageSize2_Returns200WithPagedShape()
    {
        // Arrange
        var login = await LoginAsync(ApiFactory.AdminEmail, ApiFactory.AdminPassword);
        var request = Authorized(HttpMethod.Get, "/api/products?page=0&size=2", login.AccessToken);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var page = await response.Content.ReadFromJsonAsync<ProductsListResponse>();
        Assert.NotNull(page);
        Assert.Equal(2, page.ProductsList.Count);
        Assert.Equal(0, page.PageNumber);
        Assert.Equal(2, page.PageSize);
        Assert.True(page.TotalElements >= 10);
        Assert.Equal((int)Math.Ceiling(page.TotalElements / 2.0), page.TotalPages);
    }

    [Fact]
    public async Task CreateProduct_AsAdminWithDecimalPrice_Returns201WithPrice()
    {
        // Arrange
        var login = await LoginAsync(ApiFactory.AdminEmail, ApiFactory.AdminPassword);
        var product = new ProductsDto($"SKU-{Guid.NewGuid():N}", "Test Product", 19.99m, 250, "Created by a test", "Tests", null, 5);
        var request = Authorized(HttpMethod.Post, "/api/products", login.AccessToken);
        request.Content = JsonContent.Create(product);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<ProductsResponse>();
        Assert.NotNull(created);
        Assert.Equal(19.99m, created.Price);
        Assert.Equal(product.Sku, created.Sku);
    }

    [Fact]
    public async Task CreateMyOrder_UnknownProductId_Returns404ProblemDetails()
    {
        // Arrange
        var login = await LoginAsync(ApiFactory.CustomerEmail, ApiFactory.CustomerPassword);
        var order = new OrdersDto(10.50m, "Demo Street 1", "Demo Street 1", ApiFactory.CustomerEmail,
            new DateOnly(2026, 10, 15), "Processing", [new OrderDetailsDto(10.50m, "SKU-MISSING", 1, Guid.NewGuid())]);
        var request = Authorized(HttpMethod.Post, "/api/orders/me", login.AccessToken);
        request.Content = JsonContent.Create(order);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Product does not exist in the database.", problem.Detail);
    }

    [Fact]
    public async Task Register_DuplicateEmail_Returns409ProblemDetails()
    {
        // Arrange
        var registration = NewRegistration();
        await RegisterAsync(registration);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", registration);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Register_InvalidEmail_Returns400ValidationProblem()
    {
        // Arrange
        var registration = NewRegistration() with { Email = "not-an-email" };

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", registration);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.True(problem.Errors.ContainsKey("Email"));
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
        Assert.Contains("Jwt:Secret is missing", validation.Message);
    }

    private static RegisterRequest NewRegistration() => new(
        $"{Guid.NewGuid():N}@test.com", "password123", "Test Customer", "Test Street 1", "Test Street 1", "RO", "0700000000");

    private static HttpRequestMessage Authorized(HttpMethod method, string url, string token)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new { email, password });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AuthResponse>())!;
    }

    private async Task<AuthResponse> RegisterAsync(RegisterRequest registration)
    {
        var response = await client.PostAsJsonAsync("/api/auth/register", registration);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AuthResponse>())!;
    }

    private async Task<CustomerResponse> GetMyCustomerAsync(string token)
    {
        var response = await client.SendAsync(Authorized(HttpMethod.Get, "/api/customers/me", token));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CustomerResponse>())!;
    }
}
