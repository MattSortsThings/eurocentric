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

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.AwardJuryPoints;

internal static class AwardJuryPointsFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "broadcastId")] Guid broadcastId,
        [FromBody] AwardJuryPointsRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(MapToCommand(broadcastId, requestBody),
            _ => TypedResults.NoContent(),
            cancellationToken);

    private static Command MapToCommand(Guid broadcastId, AwardJuryPointsRequest requestBody) =>
        new(broadcastId, requestBody.VotingCountryId, requestBody.RankedCompetingCountryIds);

    internal sealed record Command(Guid BroadcastId, Guid VotingCountryId, Guid[] RankedCompetingCountryIds) : ICommand<Updated>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Command, Updated>
    {
        public async Task<ErrorOr<Updated>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            var (broadcastId, votingCountryId, rankedCompetingCountryIds) = command;

            return await GetTrackedBroadcastAsync(broadcastId)
                .Then(broadcast => broadcast.AwardJuryPoints(CountryId.FromValue(votingCountryId),
                        rankedCompetingCountryIds.Select(CountryId.FromValue).ToArray())
                    .ThenDo(_ => dbContext.Broadcasts.Update(broadcast)))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(updated => updated);
        }

        private async Task<ErrorOr<Broadcast>> GetTrackedBroadcastAsync(Guid broadcastId)
        {
            BroadcastId id = BroadcastId.FromValue(broadcastId);

            Broadcast? broadcast = await dbContext.Broadcasts.AsSplitQuery()
                .Where(broadcast => broadcast.Id == id)
                .FirstOrDefaultAsync();

            return broadcast is null ? BroadcastErrors.BroadcastNotFound(id) : broadcast;
        }
    }
}
