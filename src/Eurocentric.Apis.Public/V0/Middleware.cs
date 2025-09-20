using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Features.Queryables;
using Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;
using Eurocentric.Apis.Public.V0.Features.Scoreboards;
using Microsoft.AspNetCore.Builder;
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
        RouteGroupBuilder v0Group = builder.MapGroup("v{version:apiVersion}")
            .WithName(V0Group.Name);

        v0Group.MapGetQueryableBroadcasts()
            .MapGetQueryableContests()
            .MapGetQueryableCountries();

        v0Group.MapGetCompetingCountryPointsInRangeRankings();

        v0Group.MapGetScoreboard();
    }
}
