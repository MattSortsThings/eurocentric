namespace Eurocentric.Features.Shared.Security;

public static class ApiKeyConstants
{
    public const string HttpRequestHeaderName = "X-Api-Key";
    internal const string SchemeName = "ApiKeyAuthenticationScheme";
    internal const string AppSettingsKey = "ApiKeySecurity";
}
