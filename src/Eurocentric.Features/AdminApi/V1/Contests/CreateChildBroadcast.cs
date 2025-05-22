using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using DomainBroadcast = Eurocentric.Domain.Broadcasts.Broadcast;
using Broadcast = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;
using Competitor = Eurocentric.Features.AdminApi.V1.Common.Dtos.Competitor;
using Contest = Eurocentric.Domain.Contests.Contest;
using Voter = Eurocentric.Features.AdminApi.V1.Common.Dtos.Voter;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record CreateChildBroadcastResponse(Broadcast Broadcast) : IExampleProvider<CreateChildBroadcastResponse>
{
    public static CreateChildBroadcastResponse CreateExample() => new(new Broadcast
    {
        Id = ExampleValues.BroadcastId,
        ContestId = ExampleValues.ContestId,
        ContestStage = ContestStage.GrandFinal,
        Status = BroadcastStatus.Initialized,
        Competitors =
        [
            new Competitor
            {
                CompetingCountryId = ExampleValues.CountryId1Of3,
                RunningOrderPosition = 1,
                FinishingPosition = 1,
                JuryAwards = [],
                TelevoteAwards = []
            }
        ],
        Juries = [new Voter(ExampleValues.CountryId1Of3, false)],
        Televotes = [new Voter(ExampleValues.CountryId1Of3, false)]
    });
}

public sealed record CreateChildBroadcastRequest : IExampleProvider<CreateChildBroadcastRequest>
{
    public required ContestStage ContestStage { get; init; }

    public required Guid[] CompetingCountryIds { get; init; }

    public static CreateChildBroadcastRequest CreateExample() => new()
    {
        ContestStage = ContestStage.GrandFinal, CompetingCountryIds = [ExampleValues.CountryId1Of3]
    };
}

internal static class CreateChildBroadcast
{
    internal static IEndpointRouteBuilder MapCreateChildBroadcast(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapPost("contests/{contestId:guid}/broadcasts", Endpoint.HandleAsync)
            .WithName(RouteIds.Contests.CreateChildBroadcast)
            .HasApiVersion(1, 0)
            .WithSummary("Create a child broadcast")
            .WithDescription("Creates a new broadcast for an existing contest.")
            .WithTags(EndpointTags.Contests)
            .Produces<CreateChildBroadcastResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return apiVersionGroup;
    }

    internal sealed record Command(Guid ContestId, ContestStage ContestStage, Guid[] CompetingCountryIds)
        : ICommand<CreateChildBroadcastResponse>;

    internal sealed class Handler(AppDbContext dbContext, IBroadcastIdProvider idProvider)
        : ICommandHandler<Command, CreateChildBroadcastResponse>
    {
        public async Task<ErrorOr<CreateChildBroadcastResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            (Guid contestId, ContestStage contestStage, Guid[] competingCountryIds) = command;

            return await GetTrackedContestAsync(ContestId.FromValue(contestId))
                .Then(contest => CreateBroadcast(contest, competingCountryIds.Select(CountryId.FromValue), contestStage)
                    .ThenDo(broadcast => dbContext.Broadcasts.Add(broadcast))
                    .ThenDo(broadcast => contest.AddMemo(broadcast.Id, broadcast.ContestStage))
                    .ThenDo(_ => dbContext.Contests.Update(contest)))
                .Then(broadcast => broadcast.ToBroadcastDto())
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(broadcastDto => new CreateChildBroadcastResponse(broadcastDto));
        }

        private ErrorOr<DomainBroadcast> CreateBroadcast(Contest contest,
            IEnumerable<CountryId> competingCountryIds,
            ContestStage contestStage) =>
            contestStage switch
            {
                ContestStage.SemiFinal1 => contest.CreateSemiFinal1Broadcast(competingCountryIds, idProvider.Create),
                ContestStage.SemiFinal2 => contest.CreateSemiFinal2Broadcast(competingCountryIds, idProvider.Create),
                ContestStage.GrandFinal => contest.CreateGrandFinalBroadcast(competingCountryIds, idProvider.Create),
                _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
            };

        private async Task<ErrorOr<Contest>> GetTrackedContestAsync(ContestId contestId)
        {
            Contest? contest = await dbContext.Contests.FindAsync(contestId);

            return contest is null ? ContestErrors.ContestNotFound(contestId) : contest;
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<CreatedAtRoute<CreateChildBroadcastResponse>, ProblemHttpResult>> HandleAsync(
            [FromRoute(Name = "contestId")] Guid contestId,
            [FromBody] CreateChildBroadcastRequest request,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeCommand(contestId, request)
            .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(MapToCreatedAtRoute);

        private static ErrorOr<Command> InitializeCommand(Guid contestId, CreateChildBroadcastRequest request) =>
            ErrorOrFactory.From(new Command(contestId, request.ContestStage, request.CompetingCountryIds));

        private static CreatedAtRoute<CreateChildBroadcastResponse> MapToCreatedAtRoute(CreateChildBroadcastResponse response) =>
            TypedResults.CreatedAtRoute(response,
                RouteIds.Broadcasts.GetBroadcast,
                new RouteValueDictionary { { "broadcastId", response.Broadcast.Id } });
    }
}
