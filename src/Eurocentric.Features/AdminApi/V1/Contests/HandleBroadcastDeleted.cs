using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Features.AdminApi.V1.Contests;

internal static class HandleBroadcastDeleted
{
    internal sealed class Handler(AppDbContext dbContext) : IDomainEventHandler<BroadcastDeletedEvent>
    {
        public async Task OnHandle(BroadcastDeletedEvent message, CancellationToken cancellationToken)
        {
            ContestId parentContestId = message.Broadcast.ParentContestId;

            Contest contestToUpdate = await dbContext.Contests
                .AsSplitQuery()
                .FirstAsync(contest => contest.Id == parentContestId, cancellationToken);

            contestToUpdate.RemoveMemo(message.Broadcast.Id);

            dbContext.Contests.Update(contestToUpdate);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
