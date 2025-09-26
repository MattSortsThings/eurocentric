using ErrorOr;
using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using QueryableContest = Eurocentric.Domain.V0.Views.QueryableContest;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

internal static class GetQueryableContestsV0Point1
{
    internal static IEndpointRouteBuilder MapGetQueryableContestsV0Point1(
        this IEndpointRouteBuilder builder
    )
    {
        builder
            .MapGet("v0.1/queryables/contests", ExecuteAsync)
            .WithName("PublicApi.V0.Queryables.GetQueryableContestsV0Point1")
            .WithTags(V0Group.Queryables.Tag)
            .Produces<GetQueryableContestsResponse>();

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        ErrorOr<Result> errorsOrResult = await bus.Send(
            new Query(),
            cancellationToken: cancellationToken
        );

        return MapToOk(errorsOrResult.Value);
    }

    private static Ok<GetQueryableContestsResponse> MapToOk(in Result result)
    {
        GetQueryableContestsResponse response = new(
            result.QueryableContests.Select(contest => contest.ToDto()).ToArray()
        );

        return TypedResults.Ok(response);
    }

    internal readonly record struct Result(QueryableContest[] QueryableContests);

    internal sealed record Query : IQuery<Result>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Result>
    {
        public async Task<ErrorOr<Result>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            QueryableContest[] queryableContests = await dbContext
                .QueryableContests.AsNoTracking()
                .OrderBy(contest => contest.ContestYear)
                .ToArrayAsync(cancellationToken);

            return new Result(queryableContests);
        }
    }
}
