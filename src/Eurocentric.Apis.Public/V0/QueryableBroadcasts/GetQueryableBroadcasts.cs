using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Common.Constants;
using Eurocentric.Components.Endpoints;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Placeholders.Analytics.Queryables;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;
using QueryableBroadcastItem = Eurocentric.Domain.Placeholders.Analytics.Queryables.QueryableBroadcast;
using QueryableBroadcastModel = Eurocentric.Apis.Public.V0.Common.Models.Queryables.QueryableBroadcast;

namespace Eurocentric.Apis.Public.V0.QueryableBroadcasts;

[Mapper]
internal static partial class GetQueryableBroadcastsV0Point1
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<QueryResult, DomainError> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.Match(MapToOk, MapToInternalServerError);
    }

    private static IResult MapToOk(QueryResult result) => TypedResults.Ok(result.ToResponseBody());

    private static IResult MapToInternalServerError(DomainError error) => TypedResults.InternalServerError(error.Title);

    [MapPropertyFromSource(nameof(QueryResult.QueryableBroadcasts))]
    private static partial QueryResult ToQueryResult(this List<QueryableBroadcastItem> items);

    private static partial GetQueryableBroadcastsResponseBody ToResponseBody(this QueryResult result);

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("v0.1/queryable-broadcasts", ExecuteAsync)
                .WithName(EndpointNames.GetQueryableBroadcastsV0Point1)
                .WithDisplayName(EndpointDisplayNames.GetQueryableBroadcastsV0Point1)
                .WithSummary("Get queryable broadcasts")
                .WithDescription("Lists all queryable broadcasts, ordered by broadcast date.")
                .WithTags(EndpointTags.QueryableBroadcasts)
                .Produces<GetQueryableBroadcastsResponseBody>();
        }
    }

    internal readonly record struct QueryResult(QueryableBroadcastModel[] QueryableBroadcasts);

    internal sealed record Query : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "messaging")]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            List<QueryableBroadcastItem> items = await gateway.GetQueryableBroadcastsAsync(cancellationToken);

            return items.ToQueryResult();
        }
    }
}
