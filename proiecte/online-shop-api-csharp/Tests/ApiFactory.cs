using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MySql;

namespace OnlineShopApi.Tests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public const string AdminEmail = "admin@shop.com";
    public const string AdminPassword = "admin1234";
    public const string CustomerEmail = "customer@shop.com";
    public const string CustomerPassword = "customer1234";

    private readonly MySqlContainer mysql = new MySqlBuilder("mysql:8.4").Build();

    public Task InitializeAsync() => mysql.StartAsync();

    Task IAsyncLifetime.DisposeAsync() => mysql.DisposeAsync().AsTask();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:DefaultConnection", mysql.GetConnectionString());
        builder.UseSetting("Jwt:Secret", "testing-secret-with-at-least-32-bytes-for-hs256");
        builder.UseSetting("Seed:Enabled", "true");
    }
}
