using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Mappings;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests.GetContests;

internal static class GetContestsFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(),
            TypedResults.Ok,
            cancellationToken);

    internal sealed record Query : IQuery<GetContestsResponse>;

    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetContestsResponse>
    {
        public async Task<ErrorOr<GetContestsResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Contest[] contests = await dbContext.V0Contests.AsNoTracking()
                .AsSplitQuery()
                .OrderBy(contest => contest.ContestYear)
                .Select(contest => contest.ToContestDto())
                .ToArrayAsync(cancellationToken);

            return new GetContestsResponse(contests);
        }
    }
}
