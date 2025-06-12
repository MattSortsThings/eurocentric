using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
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

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

public sealed record AwardTelevotePointsRequest : IExampleProvider<AwardTelevotePointsRequest>
{
    public required Guid VotingCountryId { get; init; }

    public required Guid[] RankedCompetingCountryIds { get; init; }

    public static AwardTelevotePointsRequest CreateExample() => new()
    {
        VotingCountryId = ExampleIds.Countries.Italy, RankedCompetingCountryIds = [ExampleIds.Countries.Austria]
    };
}

internal static class AwardTelevotePoints
{
    internal static IEndpointRouteBuilder MapAwardTelevotePoints(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPatch("broadcasts/{broadcastId:guid}/televote-points", HandleAsync)
            .WithName(EndpointIds.Broadcasts.AwardTelevotePoints)
            .WithSummary("Award televote points")
            .WithDescription("Awards the points for a single televote in an existing broadcast.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags(EndpointTags.Broadcasts);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, NoContent>> HandleAsync(
        [FromRoute(Name = "broadcastId")] Guid broadcastId,
        [FromBody] AwardTelevotePointsRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(broadcastId, requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(_ => TypedResults.NoContent());

    private static ErrorOr<Command> InitializeCommand(Guid broadcastId, AwardTelevotePointsRequest request) =>
        ErrorOrFactory.From(new Command(broadcastId, request.VotingCountryId, request.RankedCompetingCountryIds));

    internal sealed record Command(Guid BroadcastId, Guid VotingCountryId, Guid[] RankedCompetingCountryIds)
        : ICommand<Updated>;

    internal sealed class Handler(AppDbContext dbContext) : ICommandHandler<Command, Updated>
    {
        public async Task<ErrorOr<Updated>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedBroadcastAsync(BroadcastId.FromValue(command.BroadcastId))
                .Then(broadcast => broadcast.AwardTelevotePoints(CountryId.FromValue(command.VotingCountryId),
                        command.RankedCompetingCountryIds.Select(CountryId.FromValue))
                    .ThenDo(_ => dbContext.Broadcasts.Update(broadcast)))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(updated => updated);

        private async Task<ErrorOr<Broadcast>> GetTrackedBroadcastAsync(BroadcastId broadcastId)
        {
            Broadcast? broadcast = await dbContext.Broadcasts
                .AsSplitQuery()
                .Where(broadcast => broadcast.Id == broadcastId)
                .FirstOrDefaultAsync();

            return broadcast is null ? BroadcastErrors.BroadcastNotFound(broadcastId) : broadcast;
        }
    }
}
