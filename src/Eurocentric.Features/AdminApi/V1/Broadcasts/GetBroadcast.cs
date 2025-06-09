using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using BroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

public sealed record GetBroadcastResponse(BroadcastDto Broadcast);

internal static class GetBroadcast
{
    internal static IEndpointRouteBuilder MapGetBroadcast(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("broadcasts/{broadcastId:guid}", HandleAsync)
            .WithName(EndpointIds.Broadcasts.GetBroadcast)
            .WithSummary("Get a broadcast")
            .WithDescription("Retrieves a single broadcast.")
            .HasApiVersion(1, 0)
            .Produces<GetBroadcastResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags(EndpointTags.Broadcasts);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetBroadcastResponse>>> HandleAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(broadcastId)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(Guid broadcastId) => ErrorOrFactory.From(new Query(broadcastId));

    internal sealed record Query(Guid BroadcastId) : IQuery<GetBroadcastResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetBroadcastResponse>
    {
        public async Task<ErrorOr<GetBroadcastResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            BroadcastId broadcastId = BroadcastId.FromValue(query.BroadcastId);

            BroadcastDto? broadcast = await dbContext.Broadcasts
                .AsNoTracking()
                .AsSplitQuery()
                .Where(broadcast => broadcast.Id == broadcastId)
                .Select(broadcast => broadcast.ToBroadcastDto())
                .FirstOrDefaultAsync(cancellationToken);

            return broadcast is not null
                ? new GetBroadcastResponse(broadcast)
                : BroadcastErrors.BroadcastNotFound(broadcastId);
        }
    }
}
