using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.DeleteBroadcast;

internal static class DeleteBroadcastFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "broadcastId")] Guid broadcastId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Command(broadcastId), _ => TypedResults.NoContent(), cancellationToken);

    internal sealed record Command(Guid BroadcastId) : ICommand<Deleted>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Command, Deleted>
    {
        public async Task<ErrorOr<Deleted>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedBroadcastAsync(command.BroadcastId)
                .ThenDo(broadcast => dbContext.Broadcasts.Remove(broadcast))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(_ => Result.Deleted);

        private async Task<ErrorOr<Broadcast>> GetTrackedBroadcastAsync(Guid broadcastId)
        {
            BroadcastId id = BroadcastId.FromValue(broadcastId);

            Broadcast? broadcast = await dbContext.Broadcasts
                .AsSplitQuery()
                .Where(b => b.Id == id)
                .SingleOrDefaultAsync();

            return broadcast is null
                ? BroadcastErrors.BroadcastNotFound(id)
                : broadcast;
        }
    }
}
