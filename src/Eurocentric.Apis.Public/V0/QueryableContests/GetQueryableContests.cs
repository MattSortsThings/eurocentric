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
using QueryableContestItem = Eurocentric.Domain.Placeholders.Analytics.Queryables.QueryableContest;
using QueryableContestModel = Eurocentric.Apis.Public.V0.Common.Models.Queryables.QueryableContest;

namespace Eurocentric.Apis.Public.V0.QueryableContests;

[Mapper]
internal static partial class GetQueryableContestsV0Point1
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

    [MapPropertyFromSource(nameof(QueryResult.QueryableContests))]
    private static partial QueryResult ToQueryResult(this List<QueryableContestItem> items);

    private static partial GetQueryableContestsResponseBody ToResponseBody(this QueryResult result);

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("v0.1/queryable-contests", ExecuteAsync)
                .WithName(EndpointNames.GetQueryableContestsV0Point1)
                .WithDisplayName(EndpointDisplayNames.GetQueryableContestsV0Point1)
                .WithSummary("Get queryable contests")
                .WithDescription("Lists all queryable contests, ordered by contest year.")
                .WithTags(EndpointTags.QueryableContests)
                .Produces<GetQueryableContestsResponseBody>();
        }
    }

    internal readonly record struct QueryResult(QueryableContestModel[] QueryableContests);

    internal sealed record Query : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "messaging")]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            List<QueryableContestItem> items = await gateway.GetQueryableContestsAsync(cancellationToken);

            return items.ToQueryResult();
        }
    }
}
