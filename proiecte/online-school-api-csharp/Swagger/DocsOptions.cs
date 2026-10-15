namespace OnlineSchoolApi.Swagger;

public class DocsOptions
{
    public const string SectionName = "Docs";

    public string Title { get; set; } = "Online School Cloud API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = "API documentation for Online School Cloud.";
    public string BearerSchemeName { get; set; } = "bearerAuth";
    public string DefaultPassword { get; set; } = "password";
    public List<string> BootstrapUsers { get; set; } = [];
}
