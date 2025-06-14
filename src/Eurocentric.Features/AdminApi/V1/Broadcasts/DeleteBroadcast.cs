using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

internal static class DeleteBroadcast
{
    internal static IEndpointRouteBuilder MapDeleteBroadcast(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapDelete("broadcasts/{broadcastId:guid}", HandleAsync)
            .WithName(EndpointIds.Broadcasts.DeleteBroadcast)
            .WithSummary("Delete a broadcast")
            .WithDescription("Deletes a single broadcast from the system.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags(EndpointTags.Broadcasts);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, NoContent>> HandleAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(broadcastId)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(_ => TypedResults.NoContent());

    private static ErrorOr<Command> InitializeCommand(Guid broadcastId) => ErrorOrFactory.From(new Command(broadcastId));

    internal sealed record Command(Guid BroadcastId) : ICommand<Deleted>;

    internal sealed class Handler(AppDbContext dbContext) : ICommandHandler<Command, Deleted>
    {
        public async Task<ErrorOr<Deleted>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedBroadcastToDeleteAsync(BroadcastId.FromValue(command.BroadcastId))
                .ThenDo(broadcast => broadcast.AddDomainEvent(new BroadcastDeletedEvent(broadcast)))
                .ThenDo(broadcast => dbContext.Broadcasts.Remove(broadcast))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(_ => Result.Deleted);

        private async Task<ErrorOr<Broadcast>> GetTrackedBroadcastToDeleteAsync(BroadcastId broadcastId)
        {
            Broadcast? broadcast = await dbContext.Broadcasts
                .AsSplitQuery()
                .FirstOrDefaultAsync(broadcast => broadcast.Id == broadcastId);

            return broadcast is null
                ? BroadcastErrors.BroadcastNotFound(broadcastId)
                : broadcast;
        }
    }
}
