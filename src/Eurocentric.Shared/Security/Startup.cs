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
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.ConfigureOptions<ApiKeysConfigurator>()
            .AddApiKeyAuthentication()
            .AddAuthorizationPolicies();

        return services;
    }

    private static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(ApiKeyConstants.ApiKeySchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyConstants.ApiKeySchemeName, null);

        return services;
    }


    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        AuthorizationBuilder authorizationBuilder = services.AddAuthorizationBuilder();

        foreach (IAuthorizationPolicyConfigurator policy in services.GetAuthorizationPolicies())
        {
            authorizationBuilder.AddPolicy(policy.PolicyName, policy.ConfigurePolicy);
        }

        return services;
    }

    private static IAuthorizationPolicyConfigurator[] GetAuthorizationPolicies(this IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        return scope.ServiceProvider.GetRequiredService<IEnumerable<IAuthorizationPolicyConfigurator>>().ToArray();
    }
}
