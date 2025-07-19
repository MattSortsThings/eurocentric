using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Broadcast = Eurocentric.Features.AdminApi.V1.Common.Contracts.Broadcast;
using Competitor = Eurocentric.Features.AdminApi.V1.Common.Contracts.Competitor;
using Contest = Eurocentric.Domain.Aggregates.Contests.Contest;
using Voter = Eurocentric.Features.AdminApi.V1.Common.Contracts.Voter;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record CreateChildBroadcastRequest : IExampleProvider<CreateChildBroadcastRequest>
{
    /// <summary>
    ///     The broadcast's transmission date.
    /// </summary>
    public required DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The country IDs of the broadcast's competitors, in broadcast running order.
    /// </summary>
    public required Guid[] CompetingCountryIds { get; init; }

    public static CreateChildBroadcastRequest CreateExample() => new()
    {
        ContestStage = ContestStage.GrandFinal,
        BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
        CompetingCountryIds = [ExampleIds.CountryAt]
    };
}

public sealed record CreateChildBroadcastResponse(Broadcast Broadcast) : IExampleProvider<CreateChildBroadcastResponse>
{
    public static CreateChildBroadcastResponse CreateExample()
    {
        Broadcast broadcast = new()
        {
            Id = ExampleIds.Broadcast,
            ParentContestId = ExampleIds.Contest,
            ContestStage = ContestStage.GrandFinal,
            BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd"),
            Completed = false,
            Competitors =
            [
                new Competitor
                {
                    FinishingPosition = 1,
                    RunningOrderPosition = 1,
                    CompetingCountryId = ExampleIds.CountryAt,
                    JuryAwards = [],
                    TelevoteAwards = []
                }
            ],
            Juries =
            [
                new Voter { VotingCountryId = ExampleIds.CountryIt, PointsAwarded = false }
            ],
            Televotes =
            [
                new Voter { VotingCountryId = ExampleIds.CountryIt, PointsAwarded = false }
            ]
        };

        return new CreateChildBroadcastResponse(broadcast);
    }
}

internal static class CreateChildBroadcast
{
    internal static IEndpointRouteBuilder MapCreateChildBroadcast(this IEndpointRouteBuilder v1Group)
    {
        v1Group.MapPost("contests/{contestId:guid}/broadcasts", ExecuteAsync)
            .WithName(EndpointConstants.Names.Contests.CreateChildBroadcast)
            .WithSummary("Create a child broadcast")
            .WithDescription("Creates a new child broadcast for a contest.")
            .WithTags(EndpointConstants.Tags.Contests)
            .HasApiVersion(1, 0)
            .Produces<CreateChildBroadcastResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return v1Group;
    }

    private static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreateChildBroadcastResponse>>> ExecuteAsync(
        [FromRoute(Name = "contestId")] Guid contestId,
        [FromBody] CreateChildBroadcastRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(contestId, requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static ErrorOr<Command> InitializeCommand(Guid contestId, CreateChildBroadcastRequest requestBody) =>
        ErrorOrFactory.From(new Command(contestId,
            requestBody.ContestStage,
            requestBody.BroadcastDate,
            requestBody.CompetingCountryIds));

    private static CreatedAtRoute<CreateChildBroadcastResponse> MapToCreatedAtRoute(CreateChildBroadcastResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointConstants.Names.Broadcasts.GetBroadcast,
            new RouteValueDictionary { { "broadcastId", response.Broadcast.Id } });

    internal sealed record Command(Guid ContestId, ContestStage ContestStage, DateOnly BroadcastDate, Guid[] CompetingCountryIds)
        : ICommand<CreateChildBroadcastResponse>;

    internal sealed class Handler(AppDbContext dbContext, IBroadcastIdGenerator idGenerator)
        : ICommandHandler<Command, CreateChildBroadcastResponse>
    {
        public async Task<ErrorOr<CreateChildBroadcastResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            var (contestId, contestStage, broadcastDate, competingCountryIds) = command;

            return await GetTrackedContestAsync(contestId, cancellationToken)
                .Then(contest => InitializeBroadcastBuilder(contest, contestStage)
                    .ThenDo(builder => builder.WithBroadcastDate(broadcastDate)
                        .WithCompetingCountryIds(competingCountryIds.Select(CountryId.FromValue)))
                    .Then(builder => builder.Build(idGenerator.CreateSingle))
                    .ThenDo(broadcast => dbContext.Broadcasts.Add(broadcast))
                    .ThenDo(broadcast => contest.AddChildBroadcast(broadcast.Id, broadcast.ContestStage))
                    .ThenDo(_ => dbContext.Contests.Update(contest)))
                .Then(broadcast => broadcast.ToBroadcastDto())
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(broadcastDto => new CreateChildBroadcastResponse(broadcastDto));
        }

        private async Task<ErrorOr<Contest>> GetTrackedContestAsync(Guid contestId,
            CancellationToken cancellationToken = default)
        {
            ContestId id = ContestId.FromValue(contestId);

            Contest? contest = await dbContext.Contests.Where(contest => contest.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            return contest is null ? ContestErrors.ContestNotFound(id) : contest;
        }

        private static ErrorOr<BroadcastBuilder> InitializeBroadcastBuilder(Contest contest, ContestStage contestStage) =>
            contestStage switch
            {
                ContestStage.SemiFinal1 => contest.CreateSemiFinal1ChildBroadcast(),
                ContestStage.SemiFinal2 => contest.CreateSemiFinal2ChildBroadcast(),
                ContestStage.GrandFinal => contest.CreateGrandFinalChildBroadcast(),
                _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
            };
    }
}
