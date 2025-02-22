using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Eurocentric.Shared.Security;

/// <summary>
///     Authenticates API keys.
/// </summary>
/// <remarks>
///     This class is adapted from the
///     <a href="https://gist.github.com/dj-nitehawk/4efe5ef70f813aec2c55fff3bbb833c0">ApiKeyAuth</a> class by DJ-Nitehawk.
/// </remarks>
/// <param name="apiKeysOptions">Monitors <see cref="ApiKeysOptions" /> settings.</param>
/// <param name="authenticationSchemeOptions">Monitors <see cref="AuthenticationSchemeOptions" /> settings.</param>
/// <param name="logger">Creates a logger.</param>
/// <param name="urlEncoder">Represents URL character encoding.</param>
internal sealed class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeysOptions> apiKeysOptions,
    IOptionsMonitor<AuthenticationSchemeOptions> authenticationSchemeOptions,
    ILoggerFactory logger,
    UrlEncoder urlEncoder) : AuthenticationHandler<AuthenticationSchemeOptions>(authenticationSchemeOptions, logger, urlEncoder)
{
    internal const string SchemeName = "ApiKey";
    private const string ApiKeyHeaderName = "X-Api-Key";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() =>
        Task.FromResult(EndpointAllowsAnonymous() ? AuthenticateAsAnonymous() : TryAuthenticateFromApiKey());

    private AuthenticateResult AuthenticateAsAnonymous()
    {
        ClaimsIdentity identity = new([new Claim("ClientID", ClientIds.Anonymous)], Scheme.Name);
        GenericPrincipal principal = new(identity, null);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private AuthenticateResult TryAuthenticateFromApiKey()
    {
        Request.Headers.TryGetValue(ApiKeyHeaderName, out StringValues apiKey);

        return apiKey.Equals(apiKeysOptions.CurrentValue.PublicApiKey)
            ? AuthenticateAsEuroFan()
            : apiKey.Equals(apiKeysOptions.CurrentValue.AdminApiKey)
                ? AuthenticateAsAdmin()
                : AuthenticateResult.Fail("Must provide a valid API key as an \"X-Api-Key\" request header value.");
    }

    private AuthenticateResult AuthenticateAsEuroFan()
    {
        ClaimsIdentity identity = new([new Claim("ClientID", ClientIds.EuroFan)], Scheme.Name);
        GenericPrincipal principal = new(identity, [Roles.User]);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private AuthenticateResult AuthenticateAsAdmin()
    {
        ClaimsIdentity identity = new([new Claim("ClientID", ClientIds.Admin)], Scheme.Name);
        GenericPrincipal principal = new(identity, [Roles.Administrator, Roles.User]);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private bool EndpointAllowsAnonymous() =>
        Context.GetEndpoint()?.Metadata.OfType<AllowAnonymousAttribute>().Any() is null or true;
}
