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
using QueryableBroadcast = Eurocentric.Domain.V0.Views.QueryableBroadcast;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

internal static class GetQueryableBroadcastsV0Point1
{
    internal static IEndpointRouteBuilder MapGetQueryableBroadcastsV0Point1(
        this IEndpointRouteBuilder builder
    )
    {
        builder
            .MapGet("v0.1/queryables/broadcasts", ExecuteAsync)
            .WithName("PublicApi.V0.Queryables.GetQueryableBroadcastsV0Point1")
            .WithTags(V0Group.Queryables.Tag)
            .Produces<GetQueryableBroadcastsResponse>();

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

    private static Ok<GetQueryableBroadcastsResponse> MapToOk(in Result result)
    {
        GetQueryableBroadcastsResponse response = new(
            result.QueryableBroadcasts.Select(broadcast => broadcast.ToDto()).ToArray()
        );

        return TypedResults.Ok(response);
    }

    internal readonly record struct Result(QueryableBroadcast[] QueryableBroadcasts);

    internal sealed record Query : IQuery<Result>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Result>
    {
        public async Task<ErrorOr<Result>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            QueryableBroadcast[] queryableBroadcasts = await dbContext
                .QueryableBroadcasts.AsNoTracking()
                .OrderBy(broadcast => broadcast.BroadcastDate)
                .ToArrayAsync(cancellationToken);

            return new Result(queryableBroadcasts);
        }
    }
}
