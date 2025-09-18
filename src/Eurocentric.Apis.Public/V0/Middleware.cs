using Eurocentric.Apis.Public.V0.Features.Queryables;
using Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;
using Eurocentric.Apis.Public.V0.Features.Scoreboards;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the v0.x endpoints to the route builder.
    /// </summary>
    /// <param name="builder">The route builder.</param>
    internal static void MapV0Endpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGetQueryableBroadcastsV0Point1()
            .MapGetQueryableContestsV0Point1()
            .MapGetQueryableCountriesV0Point1();

        builder.MapGetQueryableBroadcastsV0Point2()
            .MapGetQueryableContestsV0Point2()
            .MapGetQueryableCountriesV0Point2();

        builder.MapGetCompetingCountryPointsInRangeRankingsV0Point2();

        builder.MapGetScoreboardV0Point2();
    }
}
