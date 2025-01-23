using ErrorOr;
using Eurocentric.Domain.Queries.VotingCountryRankings;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V0.VotingCountryRankings.GetVotingCountryPointsShareRankings;

internal static class GetVotingCountryPointsShareRankingsEndpoint
{
    internal static void MapGetVotingCountryPointsShare(this IEndpointRouteBuilder api) =>
        api.MapGet("voting-country-rankings/points-share",
                async ([AsParameters] GetVotingCountryPointsShareRankingsRequest request,
                    ISender sender,
                    CancellationToken cancellationToken = default) =>
                {
                    ErrorOr<VotingCountryPointsSharePage> result =
                        await sender.Send(request.Adapt<GetVotingCountryPointsShareRankingsQuery>(), cancellationToken);

                    return TypedResults.Ok(result.Value.Adapt<GetVotingCountryPointsShareRankingsResponse>());
                })
            .WithDisplayName("GetVotingCountryPointsShareRankings")
            .WithSummary("Get voting country points share rankings (placeholder)")
            .WithDescription("Rank each voting country by the share of the sum total points it has given to a target country, " +
                             "as a share of the maximum possible points it could have given.")
            .WithTags("Voting Country Rankings");
}
