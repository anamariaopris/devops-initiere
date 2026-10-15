using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using OneToManyApi.Cars;
using OneToManyApi.Data;
using OneToManyApi.Errors;
using OneToManyApi.Users;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");
}

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
    connectionString, new MySqlServerVersion(new Version(8, 4, 0)), mySql => mySql.EnableRetryOnFailure(maxRetryCount: 3)));
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(options => options.LoggingFields =
    HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
    HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration);
if (allowedOrigins.Length > 0)
{
    builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));
}

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpLogging();
app.UseSwagger();
app.UseSwaggerUI();
if (allowedOrigins.Length > 0)
{
    app.UseCors();
}
app.MapControllers();
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready");

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("StartupMigration");
    logger.LogInformation("Applying EF Core migrations...");
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    logger.LogInformation("EF Core migrations applied successfully.");
}

app.Run();

public partial class Program;
