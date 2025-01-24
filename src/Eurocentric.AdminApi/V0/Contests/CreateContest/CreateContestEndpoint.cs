using ErrorOr;
using Eurocentric.Domain.Entities.Contests;
using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.ErrorHandling;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Contests.CreateContest;

internal sealed class CreateContestEndpoint : ApiEndpoint
{
    public override int MajorVersion => 0;

    public override int MinorVersion => 1;

    public override RouteHandlerBuilder Map(IEndpointRouteBuilder releaseGroup) =>
        releaseGroup.MapPost("contests",
                async ([FromBody] CreateContestRequest request,
                    [FromServices] ISender sender,
                    CancellationToken cancellationToken = default) =>
                {
                    ErrorOr<Contest> errorOrValue = await sender.Send(request.Adapt<CreateContestCommand>(), cancellationToken);

                    return errorOrValue.MapToResults(MapToOkResult);
                })
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest in the system.")
            .WithTags("Contests");

    private static Ok<CreateContestResponse> MapToOkResult(Contest result) =>
        TypedResults.Ok(result.Adapt<CreateContestResponse>());
}
