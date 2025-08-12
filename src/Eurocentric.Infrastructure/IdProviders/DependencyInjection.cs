using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.IdProviders;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the domain aggregate ID provider services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddIdProviders(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IBroadcastIdProvider, BroadcastIdProvider>()
            .AddScoped<IContestIdProvider, ContestIdProvider>()
            .AddScoped<ICountryIdProvider, CountryIdProvider>();

        return services;
    }
}
