namespace Eurocentric.Features.Shared.Security;

public sealed record ApiKeysOptions
{
    public string SecretApiKey { get; set; } = null!;

    public string DemoApiKey { get; set; } = null!;
}
