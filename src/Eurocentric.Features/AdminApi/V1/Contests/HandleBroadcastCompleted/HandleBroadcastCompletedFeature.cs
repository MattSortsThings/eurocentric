using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.HandleBroadcastCompleted;

internal sealed class HandleBroadcastCompletedFeature
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(AppDbContext dbContext) : IConsumer<BroadcastCompletedEvent>
    {
        public async Task OnHandle(BroadcastCompletedEvent domainEvent, CancellationToken cancellationToken)
        {
            Broadcast broadcast = domainEvent.Broadcast;

            Contest parentContest = await dbContext.Contests.SingleAsync(contest =>
                contest.Id == broadcast.ParentContestId, cancellationToken);

            parentContest.CompleteChildBroadcast(broadcast.Id);

            dbContext.Contests.Update(parentContest);
        }


    }
}
