using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.GetContests;

internal static class GetContestsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetContestsResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetContestsResponse>
    {
        public async Task<ErrorOr<GetContestsResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Contest[] contests = await dbContext.Contests.AsNoTracking()
                .OrderBy(contest => contest.ContestYear)
                .Select(contest => contest.ToContestDto())
                .ToArrayAsync(cancellationToken);

            return new GetContestsResponse(contests);
        }
    }
}
