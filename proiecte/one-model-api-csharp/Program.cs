using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using OneModelApi.Data;
using OneModelApi.Middleware;
using OneModelApi.Repositories;
using OneModelApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

builder.Services.AddScoped<ICarCommandService, CarCommandService>();
builder.Services.AddScoped<ICarQueryService, CarQueryService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("StartupMigration");
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    logger.LogInformation("Applying EF Core migrations...");
    await db.Database.MigrateAsync();
    logger.LogInformation("EF Core migrations applied successfully.");
}

app.Run();
