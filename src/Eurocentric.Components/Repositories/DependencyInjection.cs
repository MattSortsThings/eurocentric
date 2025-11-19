using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Repositories;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the domain aggregate repository services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services
            .AddScoped<BroadcastRepository>()
            .AddScoped<IBroadcastRepository>(provider => provider.GetRequiredService<BroadcastRepository>())
            .AddScoped<IBroadcastReadRepository>(provider => provider.GetRequiredService<BroadcastRepository>())
            .AddScoped<IBroadcastWriteRepository>(provider => provider.GetRequiredService<BroadcastRepository>());

        services
            .AddScoped<ContestRepository>()
            .AddScoped<IContestRepository>(provider => provider.GetRequiredService<ContestRepository>())
            .AddScoped<IContestReadRepository>(provider => provider.GetRequiredService<ContestRepository>())
            .AddScoped<IContestWriteRepository>(provider => provider.GetRequiredService<ContestRepository>());

        services
            .AddScoped<CountryRepository>()
            .AddScoped<ICountryRepository>(provider => provider.GetRequiredService<CountryRepository>())
            .AddScoped<ICountryReadRepository>(provider => provider.GetRequiredService<CountryRepository>())
            .AddScoped<ICountryWriteRepository>(provider => provider.GetRequiredService<CountryRepository>());

        return services;
    }
}
