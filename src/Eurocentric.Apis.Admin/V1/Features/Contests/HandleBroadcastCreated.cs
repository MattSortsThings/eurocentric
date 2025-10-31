using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

internal static class HandleBroadcastCreated
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(IContestWriteRepository writeRepository)
        : IDomainEventHandler<BroadcastCreatedEvent>
    {
        public async Task OnHandle(BroadcastCreatedEvent message, CancellationToken ct)
        {
            (BroadcastId broadcastId, ContestStage contestStage, ContestId parentContestId) = (
                message.Broadcast.Id,
                message.Broadcast.ContestStage,
                message.Broadcast.ParentContestId
            );

            Contest parentContest = await GetTrackedContestAsync(parentContestId, ct);

            parentContest.AddChildBroadcast(broadcastId, contestStage);
            writeRepository.Update(parentContest);
        }

        private async Task<Contest> GetTrackedContestAsync(ContestId contestId, CancellationToken ct)
        {
            Result<Contest, IDomainError> result = await writeRepository.GetByIdAsync(contestId, ct);

            return result.GetValueOrDefault();
        }
    }
}
