namespace Eurocentric.Features.Shared.Security;

internal static class ApiKeyConstants
{
    internal const string HttpRequestHeaderName = "X-Api-Key";
    internal const string SchemeName = "ApiKeyAuthenticationScheme";
    internal const string AppSettingsKey = "ApiKeySecurity";
    internal const string BearerFormat = "API key";
}
