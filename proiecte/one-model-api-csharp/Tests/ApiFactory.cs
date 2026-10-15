using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MySql;

namespace OneModelApi.Tests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer mysql = new MySqlBuilder("mysql:8.4").Build();

    public Task InitializeAsync() => mysql.StartAsync();

    Task IAsyncLifetime.DisposeAsync() => mysql.DisposeAsync().AsTask();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:DefaultConnection", mysql.GetConnectionString());
    }
}
