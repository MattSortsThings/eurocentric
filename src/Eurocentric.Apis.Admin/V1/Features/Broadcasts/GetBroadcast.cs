using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using BroadcastAggregate = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using BroadcastDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Broadcast;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

internal static class GetBroadcast
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(broadcastId.ToQuery(), MapToOk, ct);

    private static Query ToQuery(this Guid broadcastId) => new(BroadcastId.FromValue(broadcastId));

    private static Ok<GetBroadcastResponse> MapToOk(BroadcastAggregate aggregate)
    {
        BroadcastDto broadcastDto = aggregate.ToDto();

        return TypedResults.Ok(new GetBroadcastResponse(broadcastDto));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("broadcasts/{broadcastId:guid}", ExecuteAsync)
                .WithName(V1EndpointNames.Broadcasts.GetBroadcast)
                .AddedInVersion1Point0()
                .WithSummary("Get a broadcast")
                .WithDescription("Retrieves the requested broadcast.")
                .WithTags(V1Tags.Broadcasts)
                .Produces<GetBroadcastResponse>()
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal sealed record Query(BroadcastId BroadcastId) : IQuery<BroadcastAggregate>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IBroadcastReadRepository readRepository)
        : IQueryHandler<Query, BroadcastAggregate>
    {
        public async Task<Result<BroadcastAggregate, IDomainError>> OnHandle(Query query, CancellationToken ct) =>
            await readRepository.GetUntrackedAsync(query.BroadcastId, ct);
    }
}
