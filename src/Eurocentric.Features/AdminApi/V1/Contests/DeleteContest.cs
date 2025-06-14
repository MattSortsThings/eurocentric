using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests;

internal static class DeleteContest
{
    internal static IEndpointRouteBuilder MapDeleteContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapDelete("contests/{contestId:guid}", HandleAsync)
            .WithName(EndpointIds.Contests.DeleteContest)
            .WithSummary("Delete a contest")
            .WithDescription("Deletes a single contest from the system.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, NoContent>> HandleAsync([FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(contestId)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(_ => TypedResults.NoContent());

    private static ErrorOr<Command> InitializeCommand(Guid contestId) => ErrorOrFactory.From(new Command(contestId));

    internal sealed record Command(Guid ContestId) : ICommand<Deleted>;

    internal sealed class DummyContestDeletedEventConsumer : IConsumer<ContestDeletedEvent>
    {
        public Task OnHandle(ContestDeletedEvent message, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    internal sealed class Handler(AppDbContext dbContext) : ICommandHandler<Command, Deleted>
    {
        public async Task<ErrorOr<Deleted>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedContestToDeleteAsync(ContestId.FromValue(command.ContestId))
                .ThenDo(contest => contest.RaiseContestDeletedEvent())
                .ThenDo(contest => dbContext.Contests.Remove(contest))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(_ => Result.Deleted);

        private async Task<ErrorOr<Contest>> GetTrackedContestToDeleteAsync(ContestId contestId)
        {
            Contest? contest = await dbContext.Contests
                .AsSplitQuery()
                .FirstOrDefaultAsync(contest => contest.Id == contestId);

            return contest is null
                ? ContestErrors.ContestNotFound(contestId)
                : contest.ChildBroadcasts.Count > 0
                    ? ContestErrors.CannotDeleteContest(contestId)
                    : contest;
        }
    }
}
