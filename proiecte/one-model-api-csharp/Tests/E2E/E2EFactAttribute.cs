namespace OneModelApi.Tests.E2E;

public sealed class E2EFactAttribute : FactAttribute
{
    public E2EFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("E2E_BASE_URL")))
        {
            Skip = "Set E2E_BASE_URL to run end-to-end tests against a running API.";
        }
    }
}
