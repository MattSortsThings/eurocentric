using Eurocentric.Domain.V0Analytics.Rankings.CompetingCountries;
using Eurocentric.Domain.V0Analytics.Scoreboard;
using Eurocentric.Infrastructure.AnalyticsGateways.V0.Rankings;
using Eurocentric.Infrastructure.AnalyticsGateways.V0.Scoreboards;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.AnalyticsGateways;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the analytics gateway services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAnalyticsGateways(this IServiceCollection services)
    {
        services.AddScoped<ICompetingCountryRankingsGateway, CompetingCountryRankingsGateway>()
            .AddScoped<IScoreboardGateway, ScoreboardGateway>();

        return services;
    }
}
