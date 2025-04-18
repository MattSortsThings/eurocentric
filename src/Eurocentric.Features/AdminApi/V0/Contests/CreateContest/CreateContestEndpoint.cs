using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests.CreateContest;

internal static class CreateContestEndpoint
{
    internal static void MapCreateContestV0Point2(this IEndpointRouteBuilder apiGroup) => apiGroup.MapPost("v0.2/contests",
            static async (CreateContestCommand request, IRequestResponseBus bus,
                CancellationToken cancellationToken = default) =>
            {
                ErrorOr<CreateContestResponse> result = await bus.Send(request, cancellationToken: cancellationToken);

                return TypedResults.CreatedAtRoute(result.Value, "GetContestV0Point2",
                    new RouteValueDictionary { ["contestId"] = result.Value.Contest.Id });
            }).WithSummary("Create a contest")
        .WithTags("Contests")
        .WithName("CreateContestV0Point2");
}
