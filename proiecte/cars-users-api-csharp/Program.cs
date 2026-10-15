using Microsoft.EntityFrameworkCore;
using OneToManyApi.Mapping;
using OneToManyApi.Data;
using OneToManyApi.Data.Extensions;
using OneToManyApi.Middleware;
using OneToManyApi.Repositories;
using OneToManyApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCorsPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        if (allowedOrigins.Length == 0)
        {
            throw new InvalidOperationException("Cors:AllowedOrigins must define at least one origin.");
        }

        if (allowedOrigins.Contains("*"))
        {
            if (!builder.Environment.IsDevelopment())
            {
                throw new InvalidOperationException("Wildcard CORS origin '*' is allowed only in Development.");
            }

            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            return;
        }

        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ICarCommandService, CarCommandService>();
builder.Services.AddScoped<ICarQueryService, CarQueryService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ApiCorsPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

await app.ApplyDatabaseMigrationsAsync();

app.Run();
