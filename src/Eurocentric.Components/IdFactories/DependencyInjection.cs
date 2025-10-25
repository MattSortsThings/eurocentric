using Eurocentric.Domain.Aggregates.Countries;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.IdFactories;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the ID factory services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddIdFactories(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<ICountryIdFactory, CountryIdFactory>();

        return services;
    }
}
