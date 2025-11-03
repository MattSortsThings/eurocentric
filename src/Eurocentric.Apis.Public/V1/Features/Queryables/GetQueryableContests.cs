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
using QueryableContestDto = Eurocentric.Apis.Public.V1.Dtos.Queryables.QueryableContest;
using QueryableContestRow = Eurocentric.Domain.Analytics.Queryables.QueryableContest;
using Response = Eurocentric.Apis.Public.V1.Features.Queryables.GetQueryableContestsResponse;

namespace Eurocentric.Apis.Public.V1.Features.Queryables;

internal static class GetQueryableContests
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<Response> MapToOk(QueryableContestRow[] rows)
    {
        QueryableContestDto[] dtos = rows.Select(qc => qc.ToDto()).ToArray();

        return TypedResults.Ok(new Response(dtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("queryables/contests", ExecuteAsync)
                .WithName(V1EndpointNames.Queryables.GetQueryableContests)
                .AddedInVersion1Point0()
                .WithSummary("Get all queryable contests")
                .WithDescription("Retrieves a list of all the queryable contests, ordered by contest year.")
                .WithTags(V1Tags.Queryables)
                .Produces<Response>();
        }
    }

    internal sealed record Query : IQuery<QueryableContestRow[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryableContestRow[]>
    {
        public async Task<Result<QueryableContestRow[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await gateway.GetQueryableContestsAsync(ct);
    }
}
