using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Events;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Features.AdminApi.V1.Contests.HandleBroadcastDeleted;

internal static class HandleBroadcastDeletedFeature
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(AppDbContext dbContext) : IDomainEventHandler<BroadcastDeletedEvent>
    {
        public async Task OnHandle(BroadcastDeletedEvent domainEvent, CancellationToken cancellationToken)
        {
            Broadcast broadcast = domainEvent.Broadcast;

            Contest parentContest = await dbContext.Contests.SingleAsync(contest =>
                contest.Id == broadcast.ParentContestId, cancellationToken);

            parentContest.RemoveChildBroadcast(broadcast.Id);
        }
    }
}
