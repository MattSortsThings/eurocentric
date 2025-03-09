using Eurocentric.Domain.Rules.External.DbCheckers;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.DataAccess.DbCheckers;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds all the database checker services to the application services container.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddDbCheckers(this IServiceCollection services)
    {
        services.AddScoped<ICountryDbChecker, CountryDbChecker>();

        return services;
    }
}
