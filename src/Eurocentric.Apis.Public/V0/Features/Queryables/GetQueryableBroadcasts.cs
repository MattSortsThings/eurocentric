using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Analytics.Queryables;
using Eurocentric.Domain.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;
using QueryableBroadcastDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableBroadcast;
using QueryableBroadcastRecord = Eurocentric.Domain.Analytics.Queryables.QueryableBroadcast;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

internal static class GetQueryableBroadcasts
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetQueryableBroadcastsResponse> MapToOk(QueryableBroadcastRecord[] records)
    {
        QueryableBroadcastDto[] dtos = records.Select(broadcast => broadcast.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableBroadcastsResponse(dtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("queryables/broadcasts", ExecuteAsync)
                .WithName(V0EndpointNames.Queryables.GetQueryableBroadcasts)
                .AddedInVersion0Point1()
                .WithSummary("Get all queryable broadcasts")
                .WithDescription("Retrieves all the queryable broadcasts, ordered by broadcast date.")
                .WithTags(V0Tags.Queryables)
                .Produces<GetQueryableBroadcastsResponse>();
        }
    }

    internal sealed record Query : IQuery<QueryableBroadcastRecord[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryableBroadcastRecord[]>
    {
        public async Task<Result<QueryableBroadcastRecord[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await gateway.GetQueryableBroadcastsAsync(ct);
    }
}
