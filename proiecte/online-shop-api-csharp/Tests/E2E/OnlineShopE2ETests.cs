using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using OnlineShopApi.Dtos;

namespace OnlineShopApi.Tests.E2E;

[Trait("Category", "E2E")]
public class OnlineShopE2ETests
{
    private static readonly Uri BaseUrl = new(Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:8106");

    private readonly HttpClient client = new() { BaseAddress = BaseUrl };

    private static async Task<AuthResponse> SignInAsync(string email, string password)
    {
        var anonymous = new HttpClient { BaseAddress = BaseUrl };
        var response = await anonymous.PostAsJsonAsync("/api/auth/login", new LoginRequest(email, password));
        return (await response.Content.ReadFromJsonAsync<AuthResponse>())!;
    }

    private static HttpClient WithBearer(string token)
    {
        var authenticated = new HttpClient { BaseAddress = BaseUrl };
        authenticated.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return authenticated;
    }

    private static async Task<HttpClient> LoginAsync(string email, string password) =>
        WithBearer((await SignInAsync(email, password)).AccessToken);

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
    public async Task Products_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await client.GetAsync("/api/products");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [E2EFact]
    public async Task Users_AsCustomer_ReturnsForbidden()
    {
        // Arrange
        var customer = await LoginAsync("customer@shop.com", "customer1234");

        // Act
        var response = await customer.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [E2EFact]
    public async Task PlaceOrder_AsCustomer_AppearsInMyOrders()
    {
        // Arrange
        var customer = await LoginAsync("customer@shop.com", "customer1234");
        var products = await customer.GetFromJsonAsync<ProductsListResponse>("/api/products");
        var product = products!.ProductsList[0];
        var request = new OrdersDto(
            product.Price,
            "1 E2E Street",
            "1 E2E Street",
            "customer@shop.com",
            DateOnly.FromDateTime(DateTime.UtcNow),
            "NEW",
            [new OrderDetailsDto(product.Price, product.Sku, 1, product.Id)]);

        // Act
        var created = await customer.PostAsJsonAsync("/api/orders/me", request);
        var order = await created.Content.ReadFromJsonAsync<OrdersResponse>();
        var mine = await customer.GetFromJsonAsync<OrdersListResponse>("/api/orders/me?page=0&size=100");

        // Assert
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.Contains(mine!.OrdersList, o => o.Id == order!.Id);
    }

    [E2EFact]
    public async Task RefreshToken_UsedAsBearer_ReturnsUnauthorized()
    {
        // Arrange
        var auth = await SignInAsync("customer@shop.com", "customer1234");
        var misused = WithBearer(auth.RefreshToken);

        // Act
        var response = await misused.GetAsync("/api/orders/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [E2EFact]
    public async Task ProductLifecycle_AsAdmin_CreateAndDelete()
    {
        // Arrange
        var admin = await LoginAsync("admin@shop.com", "admin1234");
        var request = new ProductsDto($"E2E-{Guid.NewGuid():N}", "E2E product", 19.99m, 100, "created by e2e", "e2e", null, 5);

        // Act
        var created = await admin.PostAsJsonAsync("/api/products", request);
        var product = await created.Content.ReadFromJsonAsync<ProductsResponse>();
        var deleted = await admin.DeleteAsync($"/api/products/{product!.Id}");
        var afterDelete = await admin.GetAsync($"/api/products/{product.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, afterDelete.StatusCode);
    }
}
