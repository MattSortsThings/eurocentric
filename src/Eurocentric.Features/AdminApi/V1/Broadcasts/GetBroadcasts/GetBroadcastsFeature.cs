using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcasts;

internal static class GetBroadcastsFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetBroadcastsResponse>;

    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetBroadcastsResponse>
    {
        public async Task<ErrorOr<GetBroadcastsResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Broadcast[] broadcasts = await dbContext.Broadcasts.AsNoTracking()
                .AsSplitQuery()
                .OrderBy(broadcast => broadcast.BroadcastDate)
                .Select(broadcast => broadcast.ToBroadcastDto())
                .ToArrayAsync(cancellationToken);

            return new GetBroadcastsResponse(broadcasts);
        }
    }
}
