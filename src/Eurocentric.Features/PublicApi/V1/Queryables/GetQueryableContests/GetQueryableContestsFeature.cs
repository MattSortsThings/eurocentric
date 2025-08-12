using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Features.PublicApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContests;

internal static class GetQueryableContestsFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetQueryableContestsResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetQueryableContestsResponse>
    {
        public async Task<ErrorOr<GetQueryableContestsResponse>> OnHandle(Query request, CancellationToken cancellationToken)
        {
            QueryableContest[] queryableContests = await dbContext.Contests.AsNoTracking()
                .AsSplitQuery()
                .Where(contest => contest.Completed)
                .OrderBy(contest => contest.ContestYear)
                .Select(contest => new QueryableContest
                {
                    ContestYear = contest.ContestYear.Value,
                    CityName = contest.CityName.Value,
                    Competitors = contest.Participants.Count(p => p.ParticipantGroup != ParticipantGroup.Zero),
                    HasRestOfWorldTelevotes = contest.ContestFormat == ContestFormat.Liverpool
                }).ToArrayAsync(cancellationToken);

            return new GetQueryableContestsResponse(queryableContests);
        }
    }
}
