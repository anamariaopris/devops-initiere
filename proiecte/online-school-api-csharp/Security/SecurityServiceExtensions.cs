using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OnlineSchoolApi.Users.Repositories;

namespace OnlineSchoolApi.Security;

public static class SecurityServiceExtensions
{
    public static IServiceCollection AddJwtSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<SecurityOptions>(configuration.GetSection(SecurityOptions.SectionName));
        services.AddSingleton<JwtService>();

        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
        var signingKey = JwtService.CreateSigningKey(jwtOptions.Secret);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var email = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                        if (string.IsNullOrWhiteSpace(email))
                        {
                            context.Fail("Token subject is missing.");
                            return;
                        }

                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var user = await userRepository.GetByEmailWithPermissionsAsync(email, context.HttpContext.RequestAborted);
                        if (user is null)
                        {
                            context.Fail("User not found.");
                            return;
                        }

                        context.Principal = JwtService.BuildPrincipal(user, JwtBearerDefaults.AuthenticationScheme);
                    }
                };
            });

        return services;
    }
}
