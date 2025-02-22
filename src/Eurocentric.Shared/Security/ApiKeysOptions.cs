namespace Eurocentric.Shared.Security;

public sealed record ApiKeysOptions
{
    public string AdminApiKey { get; set; } = string.Empty;

    public string PublicApiKey { get; set; } = string.Empty;
}
