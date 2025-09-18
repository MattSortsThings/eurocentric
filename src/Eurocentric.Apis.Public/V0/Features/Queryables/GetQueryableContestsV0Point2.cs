using System.Linq.Expressions;
using ErrorOr;
using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Domain.V0Entities;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

public static class GetQueryableContestsV0Point2
{
    private static readonly Expression<Func<Contest, QueryableContest>> MapContestToQueryableContest =
        contest => new QueryableContest
        {
            ContestYear = contest.ContestYear,
            CityName = contest.CityName,
            ParticipatingCountries = contest.Participants.Count,
            HasGlobalTelevote = contest.GlobalTelevote != null
        };

    internal static IEndpointRouteBuilder MapGetQueryableContestsV0Point2(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("v0.2/queryables/contests", ExecuteAsync)
            .WithName("PublicApi.V0.2.Queryables.GetQueryableContests")
            .WithTags(V0Group.Queryables.Tag);

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<Response> errorsOrResponse = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    public sealed record Response(QueryableContest[] QueryableContests);

    internal sealed record Query : IQuery<Response>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            IQueryable<QueryableContest> contests = dbContext.V0Contests.AsNoTracking()
                .Where(contest => contest.Queryable)
                .OrderBy(contest => contest.ContestYear)
                .Select(MapContestToQueryableContest);

            QueryableContest[] queryableContests = await contests.ToArrayAsync(cancellationToken);

            return new Response(queryableContests);
        }
    }
}
