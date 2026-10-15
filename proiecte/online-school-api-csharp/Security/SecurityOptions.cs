namespace OnlineSchoolApi.Security;

public class SecurityOptions
{
    public const string SectionName = "Security";

    public List<string> PublicPaths { get; set; } = [];
    public List<ResourceRule> ResourceRules { get; set; } = [];
    public int PasswordEncoderStrength { get; set; } = 10;
}

public class ResourceRule
{
    public string PathPattern { get; set; } = string.Empty;
    public string ReadAuthority { get; set; } = string.Empty;
    public string WriteAuthority { get; set; } = string.Empty;
}
