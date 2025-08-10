using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.DeleteContest;

internal static class DeleteContestFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Command(contestId),
            _ => TypedResults.NoContent(),
            cancellationToken);

    private static async Task<ErrorOr<Contest>> FailIfContestHasChildBroadcastsAsync(this Task<ErrorOr<Contest>> task)
    {
        ErrorOr<Contest> result = await task;

        return result.FailIf(contest => contest.ChildBroadcasts.Count != 0,
                ContestErrors.ContestDeletionBlocked())
            .Then(contest => contest);
    }

    internal sealed record Command(Guid ContestId) : ICommand<Deleted>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Command, Deleted>
    {
        public async Task<ErrorOr<Deleted>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedContestAsync(command.ContestId)
                .FailIfContestHasChildBroadcastsAsync()
                .ThenDo(contest => dbContext.Contests.Remove(contest))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(_ => Result.Deleted);

        private async Task<ErrorOr<Contest>> GetTrackedContestAsync(Guid contestId)
        {
            ContestId id = ContestId.FromValue(contestId);

            Contest? contest = await dbContext.Contests.Where(contest => contest.Id == id)
                .FirstOrDefaultAsync();

            return contest is null
                ? ContestErrors.ContestNotFound(id)
                : contest;
        }
    }
}
