using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsAverageRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsConsensusRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsInRangeRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsShareRankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Competitor Rankings".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapCompetitorRankingsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("rankings/competitors")
            .WithTags(EndpointNames.Tags.CompetitorRankings)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("points-average", GetCompetitorPointsAverageRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetitorRankings.GetCompetitorPointsAverageRankings)
            .WithSummary("Get competitor points average rankings")
            .WithDescription("Ranks every competitor in every contest broadcast by descending POINTS AVERAGE metric, i.e. " +
                             "the average value of all the individual points awards it received, expressed as a floating " +
                             "point value between 0 and 12. Optionally filters the queried data by contest year range, " +
                             "contest stage and voting method. Allows pagination overrides. Returns a page of rankings, with " +
                             "filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetitorPointsAverageRankingsResponse>();

        group.MapGet("points-consensus", GetCompetitorPointsConsensusRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetitorRankings.GetCompetitorPointsConsensusRankings)
            .WithSummary("Get competitor points consensus rankings")
            .WithDescription("Ranks every competitor in every contest broadcast by descending POINTS CONSENSUS metric, i.e. " +
                             "the cosine similarity between all the individual jury points and all the individual televote " +
                             "points awards it has received, expressed as a floating point value between 0 and 1. Uses each " +
                             "voting country in the broadcast as a vector dimension for comparison. Optionally filters the " +
                             "queried data by contest year range and contest stage. Allows pagination overrides. Returns a " +
                             "page of rankings, with filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetitorPointsConsensusRankingsResponse>();

        group.MapGet("points-in-range", GetCompetitorPointsInRangeRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetitorRankings.GetCompetitorPointsInRangeRankings)
            .WithSummary("Get competitor points in range rankings")
            .WithDescription("Ranks every competitor in every contest broadcast by descending POINTS IN RANGE metric, i.e. " +
                             "the frequency of individual points awards it has received having a value within a specified " +
                             "range, relative to the total number of points awards it received, expressed as a floating " +
                             "point value between 0 and 1. Optionally filters the queried data by contest year range, contest " +
                             "stage and voting method. Allows pagination overrides. Returns a page of rankings, with " +
                             "filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetitorPointsInRangeRankingsResponse>();

        group.MapGet("points-share", GetCompetitorPointsShareRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetitorRankings.GetCompetitorPointsShareRankings)
            .WithSummary("Get competitor points share rankings")
            .WithDescription("Ranks every competitor in every contest broadcast by descending POINTS SHARE metric, i.e. the " +
                             "total points it received, as a fraction of the maximum available points it could have received, " +
                             "expressed as a floating point value between 0 and 1. Optionally filters the queried data by " +
                             "contest year range, contest stage and voting method. Allows pagination overrides. Returns a " +
                             "page of rankings, with filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetitorPointsShareRankingsResponse>();
    }
}
