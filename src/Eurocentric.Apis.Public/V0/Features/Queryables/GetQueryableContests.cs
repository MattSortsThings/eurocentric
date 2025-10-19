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
using QueryableContestDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableContest;
using QueryableContestRecord = Eurocentric.Domain.V0.Queries.Queryables.QueryableContest;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

internal static class GetQueryableContestsV0Point2
{
    private static Ok<GetQueryableContestsResponse> MapToOk(QueryableContestRecord[] records)
    {
        QueryableContestDto[] dtos = records.Select(broadcast => broadcast.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableContestsResponse(dtos));
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        Result<QueryableContestRecord[], IDomainError> result = await bus.Send(new Query(), cancellationToken: ct);

        return result.IsSuccess
            ? MapToOk(result.GetValueOrDefault())
            : throw new InvalidOperationException("Query failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("v0.2/queryables/contests", ExecuteAsync)
                .WithName("PublicApi.V0.2.GetQueryableContests")
                .WithSummary("Get all queryable contests")
                .WithDescription("Retrieves all the queryable contests, ordered by contest year.")
                .WithTags(EndpointConstants.Tags.Queryables)
                .Produces<GetQueryableContestsResponse>();
        }
    }

    internal sealed record Query : IQuery<QueryableContestRecord[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryableContestRecord[]>
    {
        public async Task<Result<QueryableContestRecord[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await gateway.GetQueryableContestsAsync(ct);
    }
}
