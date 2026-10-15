using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data.Seed;

namespace OnlineShopApi.Data.Extensions;

public static class DatabaseStartupExtensions
{
    private const int MaxAttempts = 10;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(3);

    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigration");

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                logger.LogInformation("Applying EF Core migrations (attempt {Attempt}/{MaxAttempts})...", attempt, MaxAttempts);
                await db.Database.MigrateAsync();
                logger.LogInformation("EF Core migrations applied successfully.");
                return;
            }
            catch (Exception ex) when (attempt < MaxAttempts)
            {
                logger.LogWarning("Database not ready ({Message}). Retrying in {Delay}s...", ex.Message, RetryDelay.TotalSeconds);
                await Task.Delay(RetryDelay);
            }
        }
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        if (!app.Configuration.GetValue("Seed:Enabled", true))
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();
    }
}
