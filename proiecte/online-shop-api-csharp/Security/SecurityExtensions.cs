using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace OnlineShopApi.Security;

public static class SecurityExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration("Jwt")
            .ValidateDataAnnotations()
            .Validate(o => JwtService.SecretBytes(o.Secret).Length >= 32, "Jwt:Secret must be at least 256 bits (32 bytes) for HS256.")
            .ValidateOnStart();
        services.AddSingleton<JwtService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((bearer, jwt) =>
            {
                bearer.MapInboundClaims = false;
                bearer.TokenValidationParameters = JwtService.CreateValidationParameters(jwt.Value);
                bearer.Events = new JwtBearerEvents { OnTokenValidated = RejectNonAccessTokens };
            });
        return services;
    }

    public static IServiceCollection AddPermissionPolicies(this IServiceCollection services) =>
        services.AddAuthorization(options =>
        {
            foreach (var permission in Permissions.All)
            {
                options.AddPolicy(permission, policy => policy.RequireClaim(Permissions.ClaimType, permission));
            }

            options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        });

    public static IServiceCollection AddSwaggerWithBearer(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            var bearer = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Paste the access token from POST /api/auth/login.",
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };
            options.AddSecurityDefinition("Bearer", bearer);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { [bearer] = [] });
        });

    private static Task RejectNonAccessTokens(TokenValidatedContext context)
    {
        if (context.Principal?.FindFirst(JwtService.TypeClaim)?.Value != JwtService.AccessType)
        {
            context.Fail("Only access tokens are accepted as Bearer tokens.");
        }

        return Task.CompletedTask;
    }
}
