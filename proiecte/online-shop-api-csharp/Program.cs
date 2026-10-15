using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineShopApi.Data;
using OnlineShopApi.Data.Extensions;
using OnlineShopApi.Data.Seed;
using OnlineShopApi.Middleware;
using OnlineShopApi.Repositories;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options =>
    {
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    })
    .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = ValidationResponseFactory.Create);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Online Shop API",
        Version = "1.1.0",
        Description = "JWT-secured, permission-based Online Shop API. Login at POST /api/auth/login, then click Authorize and paste the access token. Seeded accounts: admin@shop.com / admin1234, customer@shop.com / customer1234."
    });
    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste only the JWT access token here (without the 'Bearer ' prefix). Get one from POST /api/auth/login."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" } },
            Array.Empty<string>()
        }
    });
});

var corsOrigins = CorsOriginMatcher.Parse(builder.Configuration["Cors:AllowedOrigins"]);
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCorsPolicy", policy => policy
        .SetIsOriginAllowed(origin => CorsOriginMatcher.IsAllowed(corsOrigins, origin))
        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")
        .AllowAnyHeader()
        .WithExposedHeaders("Authorization", "Content-Disposition")
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromSeconds(3600)));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36)),
        mySqlOptions => mySqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddSingleton<JwtService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = JwtService.CreateValidationParameters(jwtOptions);
        options.Events = new JwtBearerEvents { OnTokenValidated = PrincipalLoader.LoadFromDatabaseAsync };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, JsonAuthorizationResultHandler>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICustomerCommandService, CustomerCommandService>();
builder.Services.AddScoped<ICustomerQueryService, CustomerQueryService>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();
builder.Services.AddScoped<IOrderDetailCommandService, OrderDetailCommandService>();
builder.Services.AddScoped<IOrderDetailQueryService, OrderDetailQueryService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseStatusCodePages(ErrorResponses.WriteStatusCodePageAsync);

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ApiCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.ApplyDatabaseMigrationsAsync();
await app.SeedDatabaseAsync();

app.Run();
