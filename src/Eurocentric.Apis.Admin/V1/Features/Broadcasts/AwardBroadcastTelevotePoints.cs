using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

internal static class AwardBroadcastTelevotePoints
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        [FromBody] AwardBroadcastTelevotePointsRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(MapToUnitCommand(broadcastId, request), ct);

    private static UnitCommand MapToUnitCommand(Guid broadcastId, AwardBroadcastTelevotePointsRequest request)
    {
        return new UnitCommand(
            BroadcastId.FromValue(broadcastId),
            CountryId.FromValue(request.VotingCountryId),
            request.RankedCompetingCountryIds.Select(CountryId.FromValue).ToArray()
        );
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapPatch("broadcasts/{broadcastId:guid}/award-televote", ExecuteAsync)
                .WithName(V1Endpoints.Broadcasts.AwardBroadcastTelevotePoints)
                .AddedInVersion1Point0()
                .WithSummary("Award televote points in a broadcast")
                .WithDescription("Awards the points from a televote to the competitors in the requested broadcast.")
                .WithTags(V1Tags.Broadcasts)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict);
        }
    }

    internal sealed record UnitCommand(
        BroadcastId BroadcastId,
        CountryId VotingCountryId,
        IReadOnlyList<CountryId> RankedCompetingCountryIds
    ) : IUnitCommand, IAwardParams;

    [UsedImplicitly]
    internal sealed class UnitCommandHandler(IBroadcastWriteRepository writeRepository)
        : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<IDomainError>> OnHandle(UnitCommand command, CancellationToken ct)
        {
            return await GetTrackedBroadcastAsync(command.BroadcastId, ct)
                .Bind(broadcast =>
                    broadcast
                        .AwardTelevotePoints(command)
                        .Tap(() => writeRepository.Update(broadcast))
                        .Tap(() => writeRepository.SaveChangesAsync(ct))
                );
        }

        private async Task<Result<Broadcast, IDomainError>> GetTrackedBroadcastAsync(
            BroadcastId broadcastId,
            CancellationToken ct
        ) => await writeRepository.GetByIdAsync(broadcastId, ct);
    }
}
