using Eurocentric.Domain.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.DataAccess.Rules;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds all the implementations of domain rules to the application services container.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddDomainRules(this IServiceCollection services)
    {
        services.AddScoped<IUniqueCountryCodeRule, EfCoreUniqueCountryCodeRule>();

        return services;
    }
}
