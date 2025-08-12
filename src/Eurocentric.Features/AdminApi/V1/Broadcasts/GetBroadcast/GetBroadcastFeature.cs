using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Broadcast = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcast;

internal static class GetBroadcastFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "broadcastId")] Guid broadcastId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(broadcastId), TypedResults.Ok, cancellationToken);

    internal sealed record Query(Guid BroadcastId) : IQuery<GetBroadcastResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetBroadcastResponse>
    {
        public async Task<ErrorOr<GetBroadcastResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            BroadcastId broadcastId = BroadcastId.FromValue(query.BroadcastId);

            Broadcast? broadcast = await dbContext.Broadcasts.AsNoTracking()
                .AsSplitQuery()
                .Where(broadcast => broadcast.Id == broadcastId)
                .Select(broadcast => broadcast.ToBroadcastDto())
                .FirstOrDefaultAsync(cancellationToken);

            return broadcast is null
                ? BroadcastErrors.BroadcastNotFound(broadcastId)
                : new GetBroadcastResponse(broadcast);
        }
    }
}
