using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V1.Config;
using Eurocentric.Apis.Public.V1.Dtos.Queryables;
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
using QueryableBroadcastDto = Eurocentric.Apis.Public.V1.Dtos.Queryables.QueryableBroadcast;
using QueryableBroadcastRow = Eurocentric.Domain.Analytics.Queryables.QueryableBroadcast;

namespace Eurocentric.Apis.Public.V1.Features.Queryables;

internal static class GetQueryableBroadcasts
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetQueryableBroadcastsResponse> MapToOk(QueryableBroadcastRow[] rows)
    {
        QueryableBroadcastDto[] dtos = rows.Select(qc => qc.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableBroadcastsResponse(dtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("queryables/broadcasts", ExecuteAsync)
                .WithName(V1EndpointNames.Queryables.GetQueryableBroadcasts)
                .AddedInVersion1Point0()
                .WithSummary("Get all queryable broadcasts")
                .WithDescription("Retrieves a list of all the queryable broadcasts, ordered by broadcast date.")
                .WithTags(V1Tags.Queryables)
                .Produces<GetQueryableBroadcastsResponse>();
        }
    }

    internal sealed record Query : IQuery<QueryableBroadcastRow[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryableBroadcastRow[]>
    {
        public async Task<Result<QueryableBroadcastRow[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await gateway.GetQueryableBroadcastsAsync(ct);
    }
}
