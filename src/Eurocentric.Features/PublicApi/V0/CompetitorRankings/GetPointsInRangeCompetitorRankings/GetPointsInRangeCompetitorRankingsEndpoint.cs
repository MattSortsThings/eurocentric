using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.CompetitorRankings.GetPointsInRangeCompetitorRankings;

internal static class GetPointsInRangeCompetitorRankingsEndpoint
{
    internal static void MapGetPointsInRangeCompetitorRankingsV0Point1(this IEndpointRouteBuilder apiGroup) =>
        apiGroup.MapGet("v0.1/competitor-rankings/points-in-range",
                static async ([AsParameters] GetPointsInRangeCompetitorRankingsQuery query,
                    IRequestResponseBus bus,
                    CancellationToken cancellationToken = default) =>
                {
                    ErrorOr<GetPointsInRangeCompetitorRankingsResponse> result =
                        await bus.Send(query, cancellationToken: cancellationToken);

                    return TypedResults.Ok(result.Value);
                }).WithSummary("Get points in range competitor rankings")
            .WithTags("Competitor Rankings")
            .WithName("GetPointsInRangeCompetitorRankingsV0Point1");
}
