using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

internal static class HandleBroadcastCompleted
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(IContestWriteRepository writeRepository)
        : IDomainEventHandler<BroadcastCompletedEvent>
    {
        public async Task OnHandle(BroadcastCompletedEvent message, CancellationToken ct)
        {
            (BroadcastId broadcastId, ContestId parentContestId) = (
                message.Broadcast.Id,
                message.Broadcast.ParentContestId
            );

            Contest contest = await GetTrackedContestAsync(parentContestId, ct);

            contest.CompleteChildBroadcast(broadcastId);

            writeRepository.Update(contest);
        }

        private async Task<Contest> GetTrackedContestAsync(ContestId contestId, CancellationToken ct)
        {
            Result<Contest, IDomainError> result = await writeRepository.GetTrackedAsync(contestId, ct);

            return result.GetValueOrDefault();
        }
    }
}
