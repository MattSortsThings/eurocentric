using Eurocentric.Domain.Aggregates.Countries;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.IdGeneration;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the aggregate ID generation services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddIdGeneration(this IServiceCollection services)
    {
        services.AddScoped<ICountryIdGenerator, IdGenerator>();

        return services;
    }
}
