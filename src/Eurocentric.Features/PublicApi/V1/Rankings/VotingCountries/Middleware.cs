using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsAverageRankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Voting Country Rankings".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapVotingCountryRankingsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("rankings/voting-countries")
            .WithTags(EndpointNames.Tags.VotingCountryRankings)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("points-average", GetVotingCountryPointsAverageRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.VotingCountryRankings.GetVotingCountryPointsAverageRankings)
            .WithSummary("Get voting country points average rankings")
            .WithDescription("Ranks every voting country by descending POINTS AVERAGE metric, i.e. the average value of all " +
                             "the individual points awards it has given to a specified competing country, expressed as a " +
                             "floating point value between 0 and 12. Optionally filters the queried data by contest year " +
                             "range, contest stage and voting method. Allows pagination overrides. Returns a page of " +
                             "rankings, with filtering and pagination metadata.")
            .HasApiVersion(1, 0)
            .Produces<GetVotingCountryPointsAverageRankingsResponse>();
    }
}
