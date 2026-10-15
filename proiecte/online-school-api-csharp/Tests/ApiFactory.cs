using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MySql;

namespace OnlineSchoolApi.Tests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string JwtSecret = "dGVzdC1vbmx5LWp3dC1zZWNyZXQtZm9yLW9ubGluZS1zY2hvb2wtYXBpLWludGVncmF0aW9uLXRlc3Rz";

    private readonly MySqlContainer mysql = new MySqlBuilder("mysql:8.4").Build();

    public Task InitializeAsync() => mysql.StartAsync();

    Task IAsyncLifetime.DisposeAsync() => mysql.DisposeAsync().AsTask();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:DefaultConnection", mysql.GetConnectionString());
        builder.UseSetting("Jwt:Secret", JwtSecret);
        builder.UseSetting("Seed:Enabled", "true");
    }
}
