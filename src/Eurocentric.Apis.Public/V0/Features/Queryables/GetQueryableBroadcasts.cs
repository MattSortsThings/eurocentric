using System.Linq.Expressions;
using ErrorOr;
using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Apis.Public.V0.Versioning;
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
using ContestStage = Eurocentric.Apis.Public.V0.Enums.ContestStage;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

public static class GetQueryableBroadcasts
{
    private static readonly Expression<Func<Broadcast, QueryableBroadcast>> MapBroadcastToQueryableBroadcast =
        broadcast => new QueryableBroadcast
        {
            BroadcastDate = broadcast.BroadcastDate,
            ContestYear = broadcast.BroadcastDate.Year,
            ContestStage = (ContestStage)(int)broadcast.ContestStage,
            Competitors = broadcast.Competitors.Count,
            Juries = broadcast.Juries.Count,
            Televotes = broadcast.Televotes.Count
        };


    internal static IEndpointRouteBuilder MapGetQueryableBroadcasts(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/queryables/broadcasts", ExecuteAsync)
            .WithName(V0Group.Queryables.Endpoints.GetQueryableBroadcasts)
            .IntroducedInV0Point1()
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

    public sealed record Response(QueryableBroadcast[] QueryableBroadcasts);

    internal sealed record Query : IQuery<Response>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            IQueryable<Guid> contestIds = dbContext.V0Contests.AsNoTracking()
                .Where(contest => contest.Queryable)
                .Select(contest => contest.Id);

            IQueryable<QueryableBroadcast> broadcasts = dbContext.V0Broadcasts.AsNoTracking()
                .Where(broadcast => contestIds.Contains(broadcast.ParentContestId))
                .OrderBy(broadcast => broadcast.BroadcastDate)
                .Select(MapBroadcastToQueryableBroadcast);

            QueryableBroadcast[] queryableBroadcasts = await broadcasts.ToArrayAsync(cancellationToken);

            return new Response(queryableBroadcasts);
        }
    }
}
