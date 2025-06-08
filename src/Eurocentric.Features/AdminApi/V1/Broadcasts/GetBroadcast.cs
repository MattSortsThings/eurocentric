using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using DomainBroadcast = Eurocentric.Domain.Broadcasts.Broadcast;
using DomainCompetitor = Eurocentric.Domain.Broadcasts.Competitor;
using DomainTelevote = Eurocentric.Domain.Broadcasts.Televote;
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

    internal sealed class Handler : IQueryHandler<Query, GetBroadcastResponse>
    {
        public async Task<ErrorOr<GetBroadcastResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            DomainBroadcast broadcast = new(
                BroadcastId.FromValue(ExampleIds.Broadcasts.Basel2025GrandFinal),
                BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd")).Value,
                ContestId.FromValue(ExampleIds.Contests.Basel2025),
                ContestStage.GrandFinal,
                [
                    new DomainCompetitor(CountryId.FromValue(ExampleIds.Countries.Austria), 1),
                    new DomainCompetitor(CountryId.FromValue(ExampleIds.Countries.Italy), 2)
                ],
                [],
                [
                    new DomainTelevote(CountryId.FromValue(ExampleIds.Countries.Austria)),
                    new DomainTelevote(CountryId.FromValue(ExampleIds.Countries.Italy)),
                    new DomainTelevote(CountryId.FromValue(ExampleIds.Countries.RestOfTheWorld))
                ]
            );

            BroadcastDto broadcastDto = broadcast.ToBroadcastDto() with { Id = query.BroadcastId };

            return ErrorOrFactory.From(new GetBroadcastResponse(broadcastDto));
        }
    }
}
