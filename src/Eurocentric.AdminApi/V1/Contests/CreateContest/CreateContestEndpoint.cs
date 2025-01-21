using ErrorOr;
using Eurocentric.Domain.Entities.Contests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V1.Contests.CreateContest;

internal static class CreateContestEndpoint
{
    private static async Task<Ok<CreateContestResponse>> ExecuteAsync([FromBody] CreateContestRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<Contest> result = await sender.Send(request.Adapt<CreateContestCommand>(), cancellationToken);

        return TypedResults.Ok(result.Value.Adapt<CreateContestResponse>());
    }

    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder api)
    {
        api.MapPost("contests", ExecuteAsync)
            .WithDisplayName("CreateContest")
            .WithSummary("Create contest")
            .WithTags("Contests");

        return api;
    }
}
