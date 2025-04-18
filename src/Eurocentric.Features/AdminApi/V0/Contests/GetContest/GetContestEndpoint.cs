using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests.GetContest;

internal static class GetContestEndpoint
{
    internal static void MapGetContestV0Point1(this IEndpointRouteBuilder apiGroup) =>
        apiGroup.MapGet("v0.1/contests/{contestId:guid}",
                static async (Guid contestId, IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
                {
                    GetContestQuery query = new(contestId);

                    ErrorOr<GetContestResponse> result = await bus.Send(query, cancellationToken: cancellationToken);

                    return TypedResults.Ok(result.Value);
                }).WithSummary("Get a contest")
            .WithTags("Contests")
            .WithName("GetContestV0Point1");

    internal static void MapGetContestV0Point2(this IEndpointRouteBuilder apiGroup) =>
        apiGroup.MapGet("v0.2/contests/{contestId:guid}",
                static async (Guid contestId, IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
                {
                    GetContestQuery query = new(contestId);

                    ErrorOr<GetContestResponse> result = await bus.Send(query, cancellationToken: cancellationToken);

                    return TypedResults.Ok(result.Value);
                }).WithSummary("Get a contest")
            .WithTags("Contests")
            .WithName("GetContestV0Point2");
}
