namespace Eurocentric.Components.Security;

internal static class AuthenticationConstants
{
    internal const string SchemeName = "ApiKeySecurity";

    internal const string HttpRequestHeaderKey = "X-Api-Key";

    internal const string ClaimsIdentityClientIdKey = "ClientID";
}
