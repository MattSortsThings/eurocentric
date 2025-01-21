using ErrorOr;
using Eurocentric.Domain.Queries.VotingCountryRankings;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;

internal static class GetVotingCountryPointsShareEndpoint
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
            });
}
