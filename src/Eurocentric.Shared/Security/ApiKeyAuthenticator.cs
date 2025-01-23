using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Eurocentric.Shared.Security;

internal sealed class ApiKeyAuthenticator(
    IOptionsMonitor<AuthenticationSchemeOptions> authenticationSchemeOptions,
    IOptionsMonitor<ApiKeysOptions> apiKeysOptions,
    ILoggerFactory logger,
    UrlEncoder urlEncoder) : AuthenticationHandler<AuthenticationSchemeOptions>(authenticationSchemeOptions, logger, urlEncoder)
{
    internal const string SchemeName = "ApiKey";
    private const string ApiKeyRequestHeaderName = "X-Api-Key";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(ApiKeyRequestHeaderName, out StringValues extractedApiKey);

        return Task.FromResult(Authenticate(extractedApiKey));
    }

    private AuthenticateResult Authenticate(StringValues extractedApiKey) =>
        IsPublicApiKey(extractedApiKey)
            ? AuthenticateResult.Success(CreateUserAuthenticationTicket())
            : IsAdminApiKey(extractedApiKey)
                ? AuthenticateResult.Success(CreateAdminAuthenticationTicket())
                : AuthenticateResult.Fail("Must supply a valid API key as an \"X-Api-Key\" request header.");

    private AuthenticationTicket CreateAdminAuthenticationTicket()
    {
        ClaimsIdentity identity = new(
            [new Claim("ClientID", ClientIds.Admin)],
            Scheme.Name);
        GenericPrincipal principal = new(identity, [Roles.Admin]);

        return new AuthenticationTicket(principal, Scheme.Name);
    }

    private AuthenticationTicket CreateUserAuthenticationTicket()
    {
        ClaimsIdentity identity = new(
            [new Claim("ClientID", ClientIds.User)],
            Scheme.Name);
        GenericPrincipal principal = new(identity, [Roles.User]);

        return new AuthenticationTicket(principal, Scheme.Name);
    }

    private bool IsAdminApiKey(StringValues apiKey) =>
        string.Equals(apiKey, apiKeysOptions.CurrentValue.AdminApiKey, StringComparison.Ordinal);

    private bool IsPublicApiKey(StringValues apiKey) =>
        string.Equals(apiKey, apiKeysOptions.CurrentValue.PublicApiKey, StringComparison.Ordinal);
}
