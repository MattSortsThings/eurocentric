using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsAverageRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsConsensusRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsInRangeRankings;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsShareRankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Competing Country Rankings".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapCompetingCountryRankingsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("rankings/competing-countries")
            .WithTags(EndpointNames.Tags.CompetingCountryRankings)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("points-average", GetCompetingCountryPointsAverageRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetingCountryRankings.GetCompetingCountryPointsAverageRankings)
            .WithSummary("Get competing country points average rankings")
            .WithDescription("Ranks every competing country by descending POINTS AVERAGE metric, i.e. the average value of " +
                             "all the individual points awards it has received, expressed as a floating point value between " +
                             "0 and 12. Optionally filters the queried data by contest year range, contest stage, voting " +
                             "country and voting method. Allows pagination overrides. Returns a page of rankings, with " +
                             "filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsAverageRankingsResponse>();

        group.MapGet("points-consensus", GetCompetingCountryPointsConsensusRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetingCountryRankings.GetCompetingCountryPointsConsensusRankings)
            .WithSummary("Get competing country points consensus rankings")
            .WithDescription("Ranks every competing country by descending POINTS CONSENSUS metric, i.e. the cosine " +
                             "similarity between all the individual jury points and all the individual televote points awards " +
                             "it has received, expressed as a floating point value between 0 and 1. Uses each voting country " +
                             "in each contest broadcast as a vector dimension for comparison. Optionally filters the queried " +
                             "data by contest year range, contest stage and voting country. Allows pagination overrides. " +
                             "Returns a page of rankings, with filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsConsensusRankingsResponse>();

        group.MapGet("points-in-range", GetCompetingCountryPointsInRangeRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings)
            .WithSummary("Get competing country points in range rankings")
            .WithDescription("Ranks every competing country by descending POINTS IN RANGE metric, i.e. the frequency of " +
                             "individual points awards it has received having a value within a specified range, relative to " +
                             "the total number of points awards it has received, expressed as a floating point value between " +
                             "0 and 1. Optionally filters the queried data by contest year range, contest stage, voting " +
                             "country and voting method. Allows pagination overrides. Returns a page of rankings, with " +
                             "filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsInRangeRankingsResponse>();

        group.MapGet("points-share", GetCompetingCountryPointsShareRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.CompetingCountryRankings.GetCompetingCountryPointsShareRankings)
            .WithSummary("Get competing country points share rankings")
            .WithDescription("Ranks every competing country by descending POINTS SHARE metric, i.e. the total points it has " +
                             "received, as a fraction of the maximum available points it could have received, expressed as a " +
                             "floating point value between 0 and 1. Optionally filters the queried data by contest year " +
                             "range, contest stage, voting country and voting method. Allows pagination overrides. Returns a " +
                             "page of rankings, with filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsShareRankingsResponse>();
    }
}
