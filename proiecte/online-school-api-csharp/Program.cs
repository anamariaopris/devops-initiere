using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineSchoolApi.Auth;
using OnlineSchoolApi.Books;
using OnlineSchoolApi.Courses;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Enrolments;
using OnlineSchoolApi.Errors;
using OnlineSchoolApi.Swagger;
using OnlineSchoolApi.Users;
using OnlineSchoolApi.Users.Administrators;
using OnlineSchoolApi.Users.Permissions;
using OnlineSchoolApi.Users.Students;
using OnlineSchoolApi.Users.Teachers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
    connectionString, new MySqlServerVersion(new Version(8, 4, 0)), mySql => mySql.EnableRetryOnFailure(maxRetryCount: 3)));

builder.Services.AddOptions<JwtOptions>().BindConfiguration("Jwt").ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<AdministratorService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<EnrolmentService>();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<JwtService>((options, jwtService) =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = jwtService.ValidationParameters();
    });
builder.Services.AddAuthorization(options =>
{
    foreach (var authority in Enum.GetValues<Permission>().Select(p => p.ToAuthority()))
    {
        options.AddPolicy(authority, policy => policy.RequireClaim(JwtService.PermissionsClaim, authority));
    }
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod();
    if (allowedOrigins.Contains("*"))
    {
        policy.AllowAnyOrigin();
    }
    else
    {
        policy.WithOrigins(allowedOrigins).AllowCredentials();
    }
}));

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
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    logger.LogInformation("Applying EF Core migrations...");
    await db.Database.MigrateAsync();
    logger.LogInformation("EF Core migrations applied successfully.");

    if (app.Configuration.GetValue<bool>("Seed:Enabled"))
    {
        var seeded = await DataSeeder.SeedAsync(db);
        logger.LogInformation(seeded ? "Initial data seeded." : "Database already contains data, seeding skipped.");
    }
}

app.Run();

public partial class Program;
