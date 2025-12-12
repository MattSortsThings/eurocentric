using Eurocentric.Domain.Aggregates.V0;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Repositories.V0;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the placeholder repository services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>
    ///     The original <see cref="IServiceCollection" /> instance, so that method invocations can be chained.
    /// </returns>
    internal static IServiceCollection AddPlaceholderRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<ICountryRepository, CountryRepository>()
            .AddScoped<ICountryReadRepository>(sp => sp.GetRequiredService<ICountryRepository>());

        return services;
    }
}
