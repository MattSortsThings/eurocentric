using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Mappings;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests.GetContest;

internal static class GetContestFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await bus.SendWithResponseMapperAsync(new Query(contestId),
        TypedResults.Ok,
        cancellationToken);

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Guid contestId = query.ContestId;

            Contest? contest = await dbContext.V0Contests.AsNoTracking()
                .AsSplitQuery()
                .Where(contest => contest.Id == contestId)
                .Select(contest => contest.ToContestDto())
                .SingleOrDefaultAsync(cancellationToken);

            return contest is not null
                ? new GetContestResponse(contest)
                : Error.NotFound("Contest not found",
                    "No contest exists with the provided contest ID.",
                    new Dictionary<string, object> { { "contestId", contestId } });
        }
    }
}
