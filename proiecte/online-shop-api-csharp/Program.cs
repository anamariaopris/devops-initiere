using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineShopApi.Data;
using OnlineShopApi.Data.Seed;
using OnlineShopApi.Errors;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
    connectionString, new MySqlServerVersion(new Version(8, 4, 0)), mySql => mySql
        .EnableRetryOnFailure(maxRetryCount: 3)
        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderDetailService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddJwtAuthentication();
builder.Services.AddPermissionPolicies();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
    .AllowAnyHeader()
    .AllowAnyMethod()));
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithBearer();
builder.Services.AddHttpLogging(options => options.LoggingFields =
    HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
    HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false }).AllowAnonymous();
app.MapHealthChecks("/health/ready").AllowAnonymous();

app.Services.GetRequiredService<IStartupValidator>().Validate();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("StartupMigration");
    logger.LogInformation("Applying EF Core migrations...");
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    logger.LogInformation("EF Core migrations applied successfully.");

    if (app.Configuration.GetValue<bool>("Seed:Enabled"))
    {
        await scope.ServiceProvider.GetRequiredService<DataSeeder>().SeedAsync();
    }
}

app.Run();

public partial class Program;
