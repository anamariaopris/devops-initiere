using Microsoft.OpenApi.Models;

namespace OnlineSchoolApi.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithBearer(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Online School Cloud API",
                Version = "v1",
                Description = "JWT secured API. Seeded users (Seed:Enabled=true): admin@online-school.ro, " +
                              "andrei.ionescu@online-school.ro, ana.marinescu@student.online-school.ro; password: password."
            });

            var bearer = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Paste the accessToken returned by /auth/login.",
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };
            options.AddSecurityDefinition("Bearer", bearer);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { [bearer] = [] });
        });
}
