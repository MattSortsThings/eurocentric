using Eurocentric.Components.Repositories.V0;
using Eurocentric.Domain.Aggregates.Countries;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Repositories;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the domain aggregate repository services to the
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICountryReadRepository, CountryReadRepository>();

        services.AddV0Repositories();

        return services;
    }
}
