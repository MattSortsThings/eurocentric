using Eurocentric.Domain.Aggregates.Placeholders;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Repositories.Placeholders;

internal static class DependencyInjection
{
    internal static void AddPlaceholderRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<ICountryRepository, PlaceholderCountryRepository>()
            .AddScoped<ICountryReadRepository>(serviceProvider =>
                serviceProvider.GetRequiredService<ICountryRepository>()
            )
            .AddScoped<ICountryWriteRepository>(serviceProvider =>
                serviceProvider.GetRequiredService<ICountryRepository>()
            );
    }
}
