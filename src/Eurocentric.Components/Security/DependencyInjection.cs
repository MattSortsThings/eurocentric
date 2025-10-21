using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Security;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the API security services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureApiKeysOptions>();

        services
            .AddAuthentication(AuthenticationConstants.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationScheme>(
                AuthenticationConstants.SchemeName,
                null
            );

        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                nameof(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole),
                AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole
            )
            .AddPolicy(
                nameof(AuthorizationPolicies.RequireAuthenticatedClientWithUserRole),
                AuthorizationPolicies.RequireAuthenticatedClientWithUserRole
            )
            .AddFallbackPolicy(
                nameof(AuthorizationPolicies.RequireAuthenticatedClient),
                AuthorizationPolicies.RequireAuthenticatedClient
            );

        return services;
    }
}
