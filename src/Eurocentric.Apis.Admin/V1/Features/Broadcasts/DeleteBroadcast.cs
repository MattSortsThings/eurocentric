using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

internal static class DeleteBroadcast
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(broadcastId.ToUnitCommand(), ct);

    private static UnitCommand ToUnitCommand(this Guid broadcastId) => new(BroadcastId.FromValue(broadcastId));

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("broadcasts/{broadcastId:guid}", ExecuteAsync)
                .WithName(V1EndpointNames.Broadcasts.DeleteBroadcast)
                .AddedInVersion1Point0()
                .WithSummary("Delete a broadcast")
                .WithDescription("Permanently deletes the requested broadcast.")
                .WithTags(V1Tags.Broadcasts)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal sealed record UnitCommand(BroadcastId BroadcastId) : IUnitCommand;

    [UsedImplicitly]
    internal sealed class UnitCommandHandler(IBroadcastWriteRepository writeRepository, IUnitOfWork unitOfWork)
        : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<IDomainError>> OnHandle(UnitCommand command, CancellationToken ct)
        {
            return await writeRepository
                .GetTrackedAsync(command.BroadcastId, ct)
                .Tap(broadcast => broadcast.MarkForDeletion())
                .Tap(writeRepository.Remove)
                .Tap(() => unitOfWork.SaveChangesAsync(ct))
                .Bind(_ => UnitResult.Success<IDomainError>());
        }
    }
}
