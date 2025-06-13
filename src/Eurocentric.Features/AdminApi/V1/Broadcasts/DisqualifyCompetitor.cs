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

public sealed record DisqualifyCompetitorRequest(Guid CompetingCountryId) : IExampleProvider<DisqualifyCompetitorRequest>
{
    public static DisqualifyCompetitorRequest CreateExample() => new(ExampleIds.Countries.Austria);
}

internal static class DisqualifyCompetitor
{
    internal static IEndpointRouteBuilder MapDisqualifyCompetitor(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPatch("broadcasts/{broadcastId:guid}/disqualifications", HandleAsync)
            .WithName(EndpointIds.Broadcasts.DisqualifyCompetitor)
            .WithSummary("Disqualify a competitor")
            .WithDescription("Removes a single competitor from an existing broadcast before awarding points.")
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
        [FromBody] DisqualifyCompetitorRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(broadcastId, requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(_ => TypedResults.NoContent());

    private static ErrorOr<Command> InitializeCommand(Guid broadcastId, DisqualifyCompetitorRequest request) =>
        ErrorOrFactory.From(new Command(broadcastId, request.CompetingCountryId));

    internal sealed record Command(Guid BroadcastId, Guid CompetingCountryId) : ICommand<Updated>;

    internal sealed class Handler(AppDbContext dbContext) : ICommandHandler<Command, Updated>
    {
        public async Task<ErrorOr<Updated>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedBroadcastAsync(BroadcastId.FromValue(command.BroadcastId))
                .Then(broadcast => broadcast.DisqualifyCompetitor(CountryId.FromValue(command.CompetingCountryId))
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
