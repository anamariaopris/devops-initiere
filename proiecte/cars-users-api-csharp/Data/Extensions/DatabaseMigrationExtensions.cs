using Microsoft.EntityFrameworkCore;

namespace OneToManyApi.Data.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigration");

        try
        {
            await db.Database.MigrateAsync();
        }
        catch (Exception ex) when (app.Environment.IsDevelopment())
        {
            logger.LogWarning(ex, "Skipping database migration because the database is unavailable in Development.");
        }
    }
}
