using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsAverageRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsConsensusRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsInRangeRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsShareRankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V1.Rankings;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Rankings".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapRankingsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("rankings")
            .WithTags(EndpointNames.Tags.Rankings)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("competing-countries/points-average", GetCompetingCountryPointsAverageRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Rankings.GetCompetingCountryPointsAverageRankings)
            .WithSummary("Get competing country points average rankings")
            .WithDescription("Ranks each competing country by descending POINTS AVERAGE metric, " +
                             "i.e. the average value of all the individual points awards it has received. " +
                             "Returns a page of rankings.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsAverageRankingsResponse>();

        group.MapGet("competing-countries/points-consensus", GetCompetingCountryPointsConsensusRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Rankings.GetCompetingCountryPointsConsensusRankings)
            .WithSummary("Get competing country points consensus rankings")
            .WithDescription("Ranks each competing country by descending POINTS CONSENSUS metric, " +
                             "i.e. the cosine similarity between all the individual jury points awards it has received " +
                             "and all the individual televote points it has received, " +
                             "using each voting country in each broadcast as a vector dimension for comparison. " +
                             "Returns a page of rankings.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsConsensusRankingsResponse>();

        group.MapGet("competing-countries/points-in-range", GetCompetingCountryPointsInRangeRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Rankings.GetCompetingCountryPointsInRangeRankings)
            .WithSummary("Get competing country points in range rankings")
            .WithDescription("Ranks each competing country by descending POINTS IN RANGE metric, " +
                             "i.e. the relative frequency of all the individual points awards it has received having a value " +
                             "within a specified range. Returns a page of rankings.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsInRangeRankingsResponse>();

        group.MapGet("competing-countries/points-share", GetCompetingCountryPointsShareRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Rankings.GetCompetingCountryPointsShareRankings)
            .WithSummary("Get competing country points share rankings")
            .WithDescription("Ranks each competing country by descending POINTS SHARE metric, " +
                             "i.e. the total points it has received as a fraction of the available points. " +
                             "Returns a page of rankings.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsShareRankingsResponse>();
    }
}
