using Microsoft.EntityFrameworkCore;

namespace OnlineSchoolApi.Data.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("StartupMigration");
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        logger.LogInformation("Applying EF Core migrations...");
        await db.Database.MigrateAsync();
        logger.LogInformation("EF Core migrations applied successfully.");
    }
}
