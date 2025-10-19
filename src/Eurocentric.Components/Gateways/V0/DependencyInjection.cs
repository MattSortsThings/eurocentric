using Eurocentric.Domain.V0.Queries.Listings;
using Eurocentric.Domain.V0.Queries.Queryables;
using Eurocentric.Domain.V0.Queries.Rankings.CompetingCountries;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Gateways.V0;

internal static class DependencyInjection
{
    internal static IServiceCollection AddV0Gateways(this IServiceCollection services)
    {
        services
            .AddScoped<ICompetingCountryRankingsGateway, CompetingCountryRankingsGateway>()
            .AddScoped<IListingsGateway, ListingsGateway>()
            .AddScoped<IQueryablesGateway, QueryablesGateway>();

        return services;
    }
}
