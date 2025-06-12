using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
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
using Contest = Eurocentric.Domain.Contests.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record CreateChildBroadcastRequest : IExampleProvider<CreateChildBroadcastRequest>
{
    public required ContestStage ContestStage { get; init; }

    public required DateOnly BroadcastDate { get; init; }

    public required Guid[] CompetingCountryIds { get; init; }

    public static CreateChildBroadcastRequest CreateExample() => new()
    {
        ContestStage = ContestStage.GrandFinal,
        BroadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-mm-dd"),
        CompetingCountryIds = [ExampleIds.Countries.Austria, ExampleIds.Countries.Italy]
    };
}

public sealed record CreateChildBroadcastResponse(Broadcast Broadcast) : IExampleProvider<CreateChildBroadcastResponse>
{
    public static CreateChildBroadcastResponse CreateExample()
    {
        Competitor prototypeCompetitor = new()
        {
            CompetingCountryId = ExampleIds.Countries.Austria,
            RunningOrderPosition = 1,
            FinishingPosition = 1,
            TelevoteAwards = [],
            JuryAwards = []
        };

        Voter prototypeVoter = new(ExampleIds.Countries.Austria, false);

        Broadcast broadcast = Broadcast.CreateExample() with
        {
            Competitors =
            [
                prototypeCompetitor,
                prototypeCompetitor with
                {
                    CompetingCountryId = ExampleIds.Countries.Italy, RunningOrderPosition = 2, FinishingPosition = 2
                }
            ],
            Juries =
            [
                prototypeVoter,
                prototypeVoter with { VotingCountryId = ExampleIds.Countries.Italy }
            ],
            Televotes =
            [
                prototypeVoter,
                prototypeVoter with { VotingCountryId = ExampleIds.Countries.Italy },
                prototypeVoter with { VotingCountryId = ExampleIds.Countries.RestOfTheWorld }
            ]
        };

        return new CreateChildBroadcastResponse(broadcast);
    }
}

internal static class CreateChildBroadcast
{
    internal static IEndpointRouteBuilder MapCreateChildBroadcast(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPost("contests/{contestId:guid}/broadcasts", HandleAsync)
            .WithName(EndpointIds.Contests.CreateChildBroadcast)
            .WithSummary("Create a child broadcast")
            .WithDescription("Creates a new child broadcast for an existing contest.")
            .HasApiVersion(1, 0)
            .Produces<CreateChildBroadcastResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreateChildBroadcastResponse>>> HandleAsync(
        [FromRoute(Name = "contestId")] Guid contestId,
        [FromBody] CreateChildBroadcastRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(contestId, requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static ErrorOr<Command> InitializeCommand(Guid contestId, CreateChildBroadcastRequest requestBody) =>
        new Command(contestId, requestBody.ContestStage, requestBody.BroadcastDate, requestBody.CompetingCountryIds);

    private static CreatedAtRoute<CreateChildBroadcastResponse> MapToCreatedAtRoute(CreateChildBroadcastResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointIds.Broadcasts.GetBroadcast,
            new RouteValueDictionary { ["broadcastId"] = response.Broadcast.Id });

    private static BroadcastBuilder SpecifyFrom(this BroadcastBuilder builder, Command command) => builder
        .WithBroadcastDate(BroadcastDate.FromValue(command.BroadcastDate))
        .WithCompetingCountryIds(command.CompetingCountryIds.Select(CountryId.FromValue));

    internal sealed record Command(
        Guid ContestId,
        ContestStage ContestStage,
        DateOnly BroadcastDate,
        Guid[] CompetingCountryIds) : ICommand<CreateChildBroadcastResponse>;

    internal sealed class Handler(AppDbContext dbContext, IBroadcastIdGenerator idGenerator)
        : ICommandHandler<Command, CreateChildBroadcastResponse>
    {
        public async Task<ErrorOr<CreateChildBroadcastResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            ErrorOr<CreateChildBroadcastResponse> result = await GetTrackedContestAsync(ContestId.FromValue(command.ContestId))
                .Then(contest => InitializeBroadcastBuilder(contest, command.ContestStage)
                    .ThenDo(builder => builder.SpecifyFrom(command))
                    .Then(builder => builder.Build(idGenerator))
                    .ThenDo(broadcast => dbContext.Broadcasts.Add(broadcast))
                    .ThenDo(broadcast => contest.AddMemo(broadcast.Id, broadcast.ContestStage))
                    .ThenDo(_ => dbContext.Contests.Update(contest)))
                .Then(broadcast => broadcast.ToBroadcastDto())
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(broadcastDto => new CreateChildBroadcastResponse(broadcastDto));

            return result;
        }

        private static ErrorOr<BroadcastBuilder> InitializeBroadcastBuilder(Contest contest, ContestStage contestStage) =>
            contestStage switch
            {
                ContestStage.SemiFinal1 => contest.CreateSemiFinal1ChildBroadcast(),
                ContestStage.SemiFinal2 => contest.CreateSemiFinal2ChildBroadcast(),
                ContestStage.GrandFinal => contest.CreateGrandFinalChildBroadcast(),
                _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
            };

        private async Task<ErrorOr<Contest>> GetTrackedContestAsync(ContestId contestId)
        {
            Contest? contest = await dbContext.Contests
                .AsSplitQuery()
                .FirstOrDefaultAsync(contest => contest.Id == contestId);

            return contest is not null ? contest : ContestErrors.ContestNotFound(contestId);
        }
    }
}
