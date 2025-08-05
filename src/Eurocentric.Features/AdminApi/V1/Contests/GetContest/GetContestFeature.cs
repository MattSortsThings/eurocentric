using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Contest = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests.GetContest;

internal static class GetContestFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(contestId), TypedResults.Ok, cancellationToken);

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            ContestId contestId = ContestId.FromValue(query.ContestId);

            Contest? contest = await dbContext.Contests.AsNoTracking()
                .AsSplitQuery()
                .Where(contest => contest.Id == contestId)
                .Select(contest => contest.ToContestDto())
                .FirstOrDefaultAsync(cancellationToken);

            return contest is null
                ? ContestErrors.ContestNotFound(contestId)
                : new GetContestResponse(contest);
        }
    }
}
