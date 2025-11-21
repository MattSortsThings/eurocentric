using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

internal static class DeleteContest
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "contestId")] Guid contestId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(contestId.ToUnitCommand(), ct);

    private static UnitCommand ToUnitCommand(this Guid contestId) => new(ContestId.FromValue(contestId));

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("contests/{contestId:guid}", ExecuteAsync)
                .WithName(V1EndpointNames.Contests.DeleteContest)
                .AddedInVersion1Point0()
                .WithSummary("Delete a contest")
                .WithDescription("Permanently deletes the requested contest.")
                .WithTags(V1Tags.Contests)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict);
        }
    }

    internal sealed record UnitCommand(ContestId ContestId) : IUnitCommand;

    [UsedImplicitly]
    internal sealed class UnitCommandHandler(IContestWriteRepository writeRepository, IUnitOfWork unitOfWork)
        : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<IDomainError>> OnHandle(UnitCommand command, CancellationToken ct)
        {
            return await writeRepository
                .GetTrackedAsync(command.ContestId, ct)
                .Ensure(ContestInvariants.CanBeDeleted)
                .Tap(contest => contest.MarkForDeletion())
                .Tap(writeRepository.Remove)
                .Tap(() => unitOfWork.SaveChangesAsync(ct))
                .Bind(_ => UnitResult.Success<IDomainError>());
        }
    }
}
