using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.HandleBroadcastDeleted;

internal static class HandleBroadcastDeletedFeature
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(AppDbContext dbContext) : IConsumer<BroadcastDeletedEvent>
    {
        public async Task OnHandle(BroadcastDeletedEvent domainEvent, CancellationToken cancellationToken)
        {
            Broadcast broadcast = domainEvent.Broadcast;

            Contest parentContest = await dbContext.Contests.SingleAsync(contest =>
                contest.Id == broadcast.ParentContestId, cancellationToken);

            parentContest.RemoveChildBroadcast(broadcast.Id);

            dbContext.Contests.Update(parentContest);
        }
    }
}
