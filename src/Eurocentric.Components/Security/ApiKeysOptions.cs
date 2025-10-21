namespace Eurocentric.Components.Security;

/// <summary>
///     Contains options for configuring API keys.
/// </summary>
public sealed record ApiKeysOptions
{
    public string DemoApiKey { get; set; } = string.Empty;

    public string SecretApiKey { get; set; } = string.Empty;
}
