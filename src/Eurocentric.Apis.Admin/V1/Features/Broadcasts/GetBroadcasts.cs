using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

internal static class GetBroadcasts
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetBroadcastsResponse> MapToOk(ContestYear[] contestYears)
    {
        Broadcast[] broadcastDtos = contestYears
            .Select(year => year.Value)
            .Select(yearValue =>
                Broadcast.CreateExample() with
                {
                    Id = Guid.NewGuid(),
                    BroadcastDate = new DateOnly(yearValue, 1, 1),
                    ParentContestId = Guid.NewGuid(),
                }
            )
            .OrderBy(broadcast => broadcast.BroadcastDate)
            .ToArray();

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

    internal sealed record Query : IQuery<ContestYear[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, ContestYear[]>
    {
        public async Task<Result<ContestYear[], IDomainError>> OnHandle(Query _, CancellationToken ct)
        {
            await Task.CompletedTask;

            return Enumerable
                .Range(2016, 4)
                .Select(value => ContestYear.FromValue(value).GetValueOrDefault())
                .ToArray();
        }
    }
}
