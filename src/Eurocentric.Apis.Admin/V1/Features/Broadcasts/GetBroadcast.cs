using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

internal static class GetBroadcast
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(broadcastId.ToQuery(), MapToOk, ct);

    private static Query ToQuery(this Guid broadcastId) => new(broadcastId);

    private static Ok<GetBroadcastResponse> MapToOk(Guid broadcastId)
    {
        Broadcast broadcastDto = Broadcast.CreateExample() with { Id = broadcastId };

        return TypedResults.Ok(new GetBroadcastResponse(broadcastDto));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("broadcasts/{broadcastId:guid}", ExecuteAsync)
                .WithName(V1Endpoints.Broadcasts.GetBroadcast)
                .AddedInVersion1Point0()
                .WithSummary("Get a broadcast")
                .WithDescription("Retrieves the requested broadcast.")
                .WithTags(V1Tags.Broadcasts)
                .Produces<GetBroadcastResponse>()
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal sealed record Query(Guid BroadcastId) : IQuery<Guid>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, Guid>
    {
        public async Task<Result<Guid, IDomainError>> OnHandle(Query query, CancellationToken ct)
        {
            await Task.CompletedTask;

            return query.BroadcastId;
        }
    }
}
