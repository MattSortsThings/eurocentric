using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.Shared.Security;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the API key authentication and authorization services to the service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureApiKeysOptions>();

        services.AddAuthentication(ApiKeyAuthenticationScheme.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationScheme>(ApiKeyAuthenticationScheme.SchemeName, null);

        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(AuthorizationPolicies.RequireAuthenticatedUser), AuthorizationPolicies.RequireAuthenticatedUser)
            .AddPolicy(nameof(AuthorizationPolicies.RequireAuthenticatedClientWithUserRole),
                AuthorizationPolicies.RequireAuthenticatedClientWithUserRole);

        return services;
    }
}
