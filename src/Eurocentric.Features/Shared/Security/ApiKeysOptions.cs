namespace Eurocentric.Features.Shared.Security;

public sealed record ApiKeysOptions
{
    public string AdminApiKey { get; set; } = null!;

    public string PublicApiKey { get; set; } = null!;
}
