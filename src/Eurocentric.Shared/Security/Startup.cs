using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Security;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the API key authentication and role-based authorization policies to the application service descriptor
    ///     collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.ConfigureOptions<ApiKeysOptionsConfigurator>()
            .AddApiKeyAuthentication()
            .AddAuthorizationPolicies();

        return services;
    }

    private static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationHandler.SchemeName, null);

        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        AuthorizationBuilder authorizationBuilder = services.AddAuthorizationBuilder();

        foreach (IApiAuthorizationPolicy policy in services.GetAuthorizationPolicies())
        {
            authorizationBuilder.AddPolicy(policy.GetType().Name, policy.Configure);
        }

        return services;
    }

    private static IApiAuthorizationPolicy[] GetAuthorizationPolicies(this IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        return scope.ServiceProvider.GetRequiredService<IEnumerable<IApiAuthorizationPolicy>>().ToArray();
    }
}
