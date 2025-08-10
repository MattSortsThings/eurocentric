using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.HandleBroadcastDeleted;

internal static class HandleBroadcastDeletedFeature
{
    internal sealed class DomainEventHandler(AppDbContext dbContext) : IConsumer<BroadcastDeletedEvent>
    {
        public async Task OnHandle(BroadcastDeletedEvent message, CancellationToken cancellationToken)
        {
            Broadcast broadcast = message.Broadcast;

            Contest parentContest = await GetTrackedContestAsync(broadcast.ParentContestId);

            parentContest.RemoveChildBroadcast(broadcast.Id);
            dbContext.Contests.Update(parentContest);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<Contest> GetTrackedContestAsync(ContestId contestId)
        {
            Contest? contest = await dbContext.Contests.AsSplitQuery()
                .FirstOrDefaultAsync(contest => contest.Id == contestId);

            return contest ?? throw new ArgumentException("Contest not found.", nameof(contestId));
        }
    }
}
