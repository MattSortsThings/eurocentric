using Eurocentric.Domain.V0.Aggregates.Countries;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Repositories.V0;

internal static class DependencyInjection
{
    internal static IServiceCollection AddV0Repositories(this IServiceCollection services)
    {
        services
            .AddScoped<ICountryReadRepository, CountryReadRepository>()
            .AddScoped<ICountryWriteRepository, CountryWriteRepository>();

        return services;
    }
}
