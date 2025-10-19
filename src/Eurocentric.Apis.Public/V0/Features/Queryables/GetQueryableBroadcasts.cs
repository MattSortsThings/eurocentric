using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Queries.Queryables;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;
using QueryableBroadcastDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableBroadcast;
using QueryableBroadcastRecord = Eurocentric.Domain.V0.Queries.Queryables.QueryableBroadcast;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

internal static class GetQueryableBroadcastsV0Point1
{
    private static Ok<GetQueryableBroadcastsResponse> MapToOk(QueryableBroadcastRecord[] records)
    {
        QueryableBroadcastDto[] dtos = records.Select(broadcast => broadcast.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableBroadcastsResponse(dtos));
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        Result<QueryableBroadcastRecord[], IDomainError> result = await bus.Send(new Query(), cancellationToken: ct);

        return result.IsSuccess
            ? MapToOk(result.GetValueOrDefault())
            : throw new InvalidOperationException("Query failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("v0.1/queryables/broadcasts", ExecuteAsync)
                .WithName("PublicApi.V0.1.GetQueryableBroadcasts")
                .WithSummary("Get all queryable broadcasts")
                .WithDescription("Retrieves all the queryable broadcasts, ordered by broadcast date.")
                .WithTags(EndpointConstants.Tags.Queryables)
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
