using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Broadcast = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;
using Competitor = Eurocentric.Domain.Broadcasts.Competitor;

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

    internal sealed class Handler : IQueryHandler<Query, GetBroadcastResponse>
    {
        public async Task<ErrorOr<GetBroadcastResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Broadcast broadcast = CreateDummyBroadcast().ToBroadcastDto() with { Id = query.BroadcastId };

            return ErrorOrFactory.From(new GetBroadcastResponse(broadcast));
        }

        private static Domain.Broadcasts.Broadcast CreateDummyBroadcast() => new(
            BroadcastId.FromValue(ExampleValues.BroadcastId),
            ContestId.FromValue(ExampleValues.ContestId),
            ContestStage.GrandFinal,
            [
                new Competitor(CountryId.FromValue(ExampleValues.CountryId1Of3), 1)
            ], [
                new Jury(CountryId.FromValue(ExampleValues.CountryId2Of3))
            ], [
                new Televote(CountryId.FromValue(ExampleValues.CountryId2Of3))
            ]);
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
