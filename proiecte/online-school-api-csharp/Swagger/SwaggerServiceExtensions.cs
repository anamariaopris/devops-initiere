using Microsoft.OpenApi.Models;

namespace OnlineSchoolApi.Swagger;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var docs = configuration.GetSection(DocsOptions.SectionName).Get<DocsOptions>() ?? new DocsOptions();
        var usersSummary = docs.BootstrapUsers.Count == 0
            ? "No bootstrap users configured."
            : string.Join(", ", docs.BootstrapUsers);

        var description = $"""
            {docs.Description}

            Bootstrap users available after the initial database migration:
            {usersSummary}

            Default password for all bootstrap users:
            {docs.DefaultPassword}

            Protected endpoints use Bearer JWT tokens and each operation documents the required permission.
            """;

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = docs.Title,
                Version = docs.Version,
                Description = description
            });

            options.AddSecurityDefinition(docs.BearerSchemeName, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the JWT token returned by /auth/login."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = docs.BearerSchemeName }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
