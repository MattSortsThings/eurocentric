using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Broadcast = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

public sealed record GetBroadcastResponse(Broadcast Broadcast);

internal static class GetBroadcast
{
    internal static IEndpointRouteBuilder MapGetBroadcast(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("broadcasts/{broadcastId:guid}", Endpoint.HandleAsync)
            .WithName(RouteIds.Broadcasts.GetBroadcast)
            .HasApiVersion(1, 0)
            .WithSummary("Get a broadcast")
            .WithDescription("Retrieves a single broadcast.")
            .WithTags(EndpointTags.Broadcasts)
            .Produces<GetBroadcastResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiVersionGroup;
    }

    internal sealed record Query(Guid BroadcastId) : IQuery<GetBroadcastResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetBroadcastResponse>
    {
        public async Task<ErrorOr<GetBroadcastResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            BroadcastId broadcastId = BroadcastId.FromValue(query.BroadcastId);

            Broadcast? broadcast = await dbContext.Broadcasts.AsNoTracking()
                .Where(broadcast => broadcast.Id == broadcastId)
                .Select(broadcast => broadcast.ToBroadcastDto())
                .FirstOrDefaultAsync(cancellationToken);

            return broadcast is null ? BroadcastErrors.BroadcastNotFound(broadcastId) : new GetBroadcastResponse(broadcast);
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<GetBroadcastResponse>, ProblemHttpResult>> HandleAsync(
            [FromRoute(Name = "broadcastId")] Guid broadcastId,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery(broadcastId)
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery(Guid contestId) => ErrorOrFactory.From(new Query(contestId));
    }
}
