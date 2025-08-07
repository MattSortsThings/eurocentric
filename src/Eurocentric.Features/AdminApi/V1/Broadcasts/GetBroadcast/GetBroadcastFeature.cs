using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;
using BroadcastAggregate = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcast;

internal static class GetBroadcastFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "broadcastId")] Guid broadcastId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(broadcastId), TypedResults.Ok, cancellationToken);

    internal sealed record Query(Guid BroadcastId) : IQuery<GetBroadcastResponse>;

    [UsedImplicitly]
    internal sealed class Handler : IQueryHandler<Query, GetBroadcastResponse>
    {
        public async Task<ErrorOr<GetBroadcastResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            BroadcastAggregate dummyBroadcast = BroadcastAggregate.CreateDummyBroadcast();

            return new GetBroadcastResponse(dummyBroadcast.ToBroadcastDto() with { Id = query.BroadcastId });
        }
    }
}
