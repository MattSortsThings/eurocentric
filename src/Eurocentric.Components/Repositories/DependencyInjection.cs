using Eurocentric.Components.Repositories.V0;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
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
        services
            .AddScoped<IContestReadRepository, ContestReadRepository>()
            .AddScoped<ICountryReadRepository, CountryReadRepository>()
            .AddScoped<ICountryWriteRepository, CountryWriteRepository>();

        services.AddV0Repositories();

        return services;
    }
}
