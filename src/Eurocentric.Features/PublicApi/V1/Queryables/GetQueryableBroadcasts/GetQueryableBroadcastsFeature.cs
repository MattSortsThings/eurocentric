using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Features.PublicApi.V1.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableBroadcasts;

internal static class GetQueryableBroadcastsFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetQueryableBroadcastsResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetQueryableBroadcastsResponse>
    {
        public async Task<ErrorOr<GetQueryableBroadcastsResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            IQueryable<Contest> queryableContests = dbContext.Contests.AsNoTracking()
                .AsSplitQuery()
                .Where(contest => contest.Completed);

            QueryableBroadcast[] queryableBroadcasts = await dbContext.Broadcasts.AsNoTracking()
                .AsSplitQuery()
                .OrderBy(b => b.BroadcastDate)
                .Join(queryableContests,
                    broadcast => broadcast.ParentContestId,
                    contest => contest.Id,
                    (broadcast, contest) => new QueryableBroadcast
                    {
                        ContestYear = contest.ContestYear.Value,
                        ContestStage = (ContestStage)(int)broadcast.ContestStage,
                        BroadcastDate = broadcast.BroadcastDate.Value,
                        Competitors = broadcast.Competitors.Count,
                        Juries = broadcast.Juries.Count,
                        Televotes = broadcast.Televotes.Count
                    })
                .ToArrayAsync(cancellationToken);

            return new GetQueryableBroadcastsResponse(queryableBroadcasts);
        }
    }
}
