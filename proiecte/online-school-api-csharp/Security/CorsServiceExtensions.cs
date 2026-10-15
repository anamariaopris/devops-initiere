namespace OnlineSchoolApi.Security;

public static class CorsServiceExtensions
{
    public const string PolicyName = "ApiCorsPolicy";

    public static IServiceCollection AddApiCors(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>() ?? new CorsSettings();
        var originPatterns = settings.AllOriginPatterns().Select(p => new OriginPattern(p)).ToList();

        services.AddCors(options => options.AddPolicy(PolicyName, policy =>
        {
            policy.SetIsOriginAllowed(origin => originPatterns.Any(p => p.Matches(origin)));

            if (settings.AllowedMethods.Count == 0)
            {
                policy.AllowAnyMethod();
            }
            else
            {
                policy.WithMethods(settings.AllowedMethods.ToArray());
            }

            if (settings.AllowedHeaders.Count == 0)
            {
                policy.AllowAnyHeader();
            }
            else
            {
                policy.WithHeaders(settings.AllowedHeaders.ToArray());
            }

            policy
                .WithExposedHeaders(settings.ExposedHeaders.ToArray())
                .SetPreflightMaxAge(TimeSpan.FromSeconds(settings.MaxAgeSeconds));

            if (settings.AllowCredentials)
            {
                policy.AllowCredentials();
            }
        }));

        return services;
    }
}
