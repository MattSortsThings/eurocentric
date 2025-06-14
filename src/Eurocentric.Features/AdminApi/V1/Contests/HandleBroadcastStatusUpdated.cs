using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Features.AdminApi.V1.Contests;

internal static class HandleBroadcastStatusUpdated
{
    internal sealed class Handler(AppDbContext dbContext) : IDomainEventHandler<BroadcastStatusUpdatedEvent>
    {
        public async Task OnHandle(BroadcastStatusUpdatedEvent message, CancellationToken cancellationToken)
        {
            ContestId parentContestId = message.Broadcast.ParentContestId;
            BroadcastId broadcastId = message.Broadcast.Id;
            BroadcastStatus broadcastStatus = message.Broadcast.BroadcastStatus;

            Contest contestToUpdate = await dbContext.Contests
                .AsSplitQuery()
                .FirstAsync(contest => contest.Id == parentContestId, cancellationToken);

            contestToUpdate.ReplaceMemo(broadcastId, broadcastStatus);

            dbContext.Contests.Update(contestToUpdate);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
