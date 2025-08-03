using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Eurocentric.Features.Shared.Security;

/// <summary>
///     Authenticates an HTTP request using an API key obtained from a request header.
/// </summary>
/// <remarks>
///     This class is adapted from the
///     <a href="https://gist.github.com/dj-nitehawk/4efe5ef70f813aec2c55fff3bbb833c0">ApiKeyAuth</a> class by DJ-Nitehawk.
/// </remarks>
/// <param name="apiKeysOptions">Monitors <see cref="ApiKeySecurityOptions" /> settings.</param>
/// <param name="authenticationSchemeOptions">Monitors <see cref="AuthenticationSchemeOptions" /> settings.</param>
/// <param name="logger">Creates a logger.</param>
/// <param name="urlEncoder">Represents URL character encoding.</param>
internal sealed class ApiKeyAuthenticationScheme(
    IOptionsMonitor<ApiKeySecurityOptions> apiKeysOptions,
    IOptionsMonitor<AuthenticationSchemeOptions> authenticationSchemeOptions,
    ILoggerFactory logger,
    UrlEncoder urlEncoder) : AuthenticationHandler<AuthenticationSchemeOptions>(authenticationSchemeOptions, logger, urlEncoder)
{
    private const string ClaimsIdentityClientIdKey = "ClientID";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult result = EndpointAllowsAnonymous()
            ? AuthenticateAsAnonymous()
            : TryAuthenticateFromApiKey();

        return Task.FromResult(result);
    }

    private bool EndpointAllowsAnonymous() =>
        Context.GetEndpoint()?.Metadata.OfType<AllowAnonymousAttribute>().Any() is null or true;

    private AuthenticateResult TryAuthenticateFromApiKey()
    {
        Request.Headers.TryGetValue(ApiKeyConstants.HttpRequestHeaderName, out StringValues apiKey);

        return apiKey.Equals(apiKeysOptions.CurrentValue.DemoApiKey)
            ? AuthenticateAsUser()
            : apiKey.Equals(apiKeysOptions.CurrentValue.SecretApiKey)
                ? AuthenticateAsAdministrator()
                : AuthenticateResult.Fail("Must provide a valid API key as an \"X-Api-Key\" request header value.");
    }

    private AuthenticateResult AuthenticateAsAdministrator()
    {
        ClaimsIdentity identity = new([new Claim(ClaimsIdentityClientIdKey, ClientIds.Administrator)], Scheme.Name);
        GenericPrincipal principal = new(identity, [Roles.Administrator, Roles.User]);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private AuthenticateResult AuthenticateAsAnonymous()
    {
        ClaimsIdentity identity = new([new Claim(ClaimsIdentityClientIdKey, ClientIds.Anonymous)], Scheme.Name);
        GenericPrincipal principal = new(identity, null);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private AuthenticateResult AuthenticateAsUser()
    {
        ClaimsIdentity identity = new([new Claim(ClaimsIdentityClientIdKey, ClientIds.User)], Scheme.Name);
        GenericPrincipal principal = new(identity, [Roles.User]);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
