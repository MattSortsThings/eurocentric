using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests.GetContests;

internal static class GetContestsEndpoint
{
    internal static void MapGetContestsV0Point1(this IEndpointRouteBuilder apiGroup) => apiGroup.MapGet("v0.1/contests",
            static async (IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
            {
                ErrorOr<GetContestsResponse> result =
                    await bus.Send(new GetContestsQuery(), cancellationToken: cancellationToken);

                return TypedResults.Ok(result.Value);
            }).WithSummary("Get contests")
        .WithTags("Contests")
        .WithName("GetContestsV0Point1");

    internal static void MapGetContestsV0Point2(this IEndpointRouteBuilder apiGroup) => apiGroup.MapGet("v0.2/contests",
            static async (IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
            {
                ErrorOr<GetContestsResponse> result =
                    await bus.Send(new GetContestsQuery(), cancellationToken: cancellationToken);

                return TypedResults.Ok(result.Value);
            }).WithSummary("Get contests")
        .WithTags("Contests")
        .WithName("GetContestsV0Point2");
}
