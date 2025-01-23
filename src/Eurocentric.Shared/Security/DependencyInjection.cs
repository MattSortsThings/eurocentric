using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Security;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the API security services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">The application service descriptor collection to be modified.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.ConfigureOptions<ApiKeysOptionsConfigurator>();

        services.AddAuthentication(ApiKeyAuthenticator.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticator>(ApiKeyAuthenticator.SchemeName, null);

        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(AuthorizationPolicies.AuthenticatedAdminOnly),
                AuthorizationPolicies.AuthenticatedAdminOnly.ConfigurePolicy)
            .AddPolicy(nameof(AuthorizationPolicies.AuthenticatedAdminOrUserOnly),
                AuthorizationPolicies.AuthenticatedAdminOrUserOnly.ConfigurePolicy);

        return services;
    }
}
