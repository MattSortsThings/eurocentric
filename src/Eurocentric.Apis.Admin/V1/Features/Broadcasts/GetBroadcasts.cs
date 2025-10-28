using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
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

internal static class GetBroadcasts
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetBroadcastsResponse> MapToOk(BroadcastAggregate[] aggregates)
    {
        BroadcastDto[] broadcastDtos = aggregates.Select(broadcast => broadcast.ToDto()).ToArray();

        return TypedResults.Ok(new GetBroadcastsResponse(broadcastDtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("broadcasts", ExecuteAsync)
                .WithName(V1Endpoints.Broadcasts.GetBroadcasts)
                .AddedInVersion1Point0()
                .WithSummary("Get all broadcasts")
                .WithDescription("Retrieves a list of all existing broadcasts, ordered by broadcast date.")
                .WithTags(V1Tags.Broadcasts)
                .Produces<GetBroadcastsResponse>();
        }
    }

    internal sealed record Query : IQuery<BroadcastAggregate[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IBroadcastReadRepository readRepository)
        : IQueryHandler<Query, BroadcastAggregate[]>
    {
        public async Task<Result<BroadcastAggregate[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await readRepository.GetAllAsync(ct);
    }
}
