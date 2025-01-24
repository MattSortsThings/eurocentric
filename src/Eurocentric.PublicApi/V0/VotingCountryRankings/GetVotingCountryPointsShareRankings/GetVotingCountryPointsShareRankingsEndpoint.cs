using ErrorOr;
using Eurocentric.Domain.Queries.VotingCountryRankings;
using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.ErrorHandling;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V0.VotingCountryRankings.GetVotingCountryPointsShareRankings;

internal sealed class GetVotingCountryPointsShareRankingsEndpoint : ApiEndpoint
{
    public override int MajorVersion => 0;

    public override int MinorVersion => 1;

    public override RouteHandlerBuilder Map(IEndpointRouteBuilder releaseGroup) =>
        releaseGroup.MapGet("voting-country-rankings/points-share",
                async ([AsParameters] GetVotingCountryPointsShareRankingsRequest request,
                    [FromServices] ISender sender,
                    CancellationToken cancellationToken = default) =>
                {
                    ErrorOr<VotingCountryPointsSharePage> errorOrValue =
                        await sender.Send(request.Adapt<GetVotingCountryPointsShareRankingsQuery>(), cancellationToken);

                    return errorOrValue.MapToResults(MapToOkResult);
                })
            .WithSummary("Get voting country points share rankings")
            .WithDescription("Ranks each voting country by the share of the possible points that it has awarded to a target " +
                             "country. Returns a page of rankings with query metadata.")
            .WithTags("Voting Country Rankings");

    private static Ok<GetVotingCountryPointsShareRankingsResponse> MapToOkResult(VotingCountryPointsSharePage result) =>
        TypedResults.Ok(result.Adapt<GetVotingCountryPointsShareRankingsResponse>());
}
