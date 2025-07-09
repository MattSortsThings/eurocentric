namespace Eurocentric.Features.Shared.Security;

public sealed class ApiKeySecurityOptions
{
    public string DemoApiKey { get; set; } = string.Empty;

    public string SecretApiKey { get; set; } = string.Empty;
}
