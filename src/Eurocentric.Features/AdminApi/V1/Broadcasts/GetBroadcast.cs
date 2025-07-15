using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

public sealed record GetBroadcastResponse(Broadcast Broadcast);

internal static class GetBroadcast
{
    internal static IEndpointRouteBuilder MapGetBroadcast(this IEndpointRouteBuilder v1Group)
    {
        v1Group.MapGet("broadcasts/{broadcastId:guid}", ExecuteAsync)
            .WithName(EndpointConstants.Names.Broadcasts.GetBroadcast)
            .WithSummary("Get a broadcast")
            .WithDescription("Retrieves a single broadcast.")
            .WithTags(EndpointConstants.Tags.Broadcasts)
            .HasApiVersion(1, 0)
            .Produces<GetBroadcastResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return v1Group;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetBroadcastResponse>>> ExecuteAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(broadcastId)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(Guid broadcastId) => ErrorOrFactory.From(new Query(broadcastId));

    internal sealed record Query(Guid BroadcastId) : IQuery<GetBroadcastResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetBroadcastResponse>
    {
        public async Task<ErrorOr<GetBroadcastResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Broadcast dummyBroadcast = DummyBroadcastGenerator.Create().ToBroadcastDto() with { Id = query.BroadcastId };

            return ErrorOrFactory.From(new GetBroadcastResponse(dummyBroadcast));
        }
    }
}
