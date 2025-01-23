using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Contest = Eurocentric.Domain.Entities.Contests.Contest;

namespace Eurocentric.AdminApi.V0.Contests.CreateContest;

internal static class CreateContestEndpoint
{
    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder api)
    {
        api.MapPost("contests",
                async ([FromBody] CreateContestRequest request,
                    [FromServices] ISender sender,
                    CancellationToken cancellationToken = default) =>
                {
                    ErrorOr<Contest> result = await sender.Send(request.Adapt<CreateContestCommand>(), cancellationToken);

                    return TypedResults.Ok(result.Value.Adapt<CreateContestResponse>());
                })
            .WithDisplayName("CreateContest")
            .WithSummary("Create contest (placeholder)")
            .WithDescription("Create a new contest")
            .WithTags("Contests")
            .Produces<CreateContestResponse>();

        return api;
    }
}
