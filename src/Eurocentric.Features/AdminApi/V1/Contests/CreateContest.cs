using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
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
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record CreateContestResponse(ContestDto Contest) : IExampleProvider<CreateContestResponse>
{
    public static CreateContestResponse CreateExample()
    {
        ContestDto contest = ContestDto.CreateExample() with { ContestStatus = ContestStatus.Initialized, ChildBroadcasts = [] };

        return new CreateContestResponse(contest);
    }
}

public sealed record CreateContestRequest : IExampleProvider<CreateContestRequest>
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public Guid? Group0CountryId { get; init; }

    public required ParticipantSpecification[] Group1Participants { get; init; }

    public required ParticipantSpecification[] Group2Participants { get; init; }

    public static CreateContestRequest CreateExample() => new()
    {
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        Group0CountryId = ExampleIds.Countries.RestOfTheWorld,
        Group1Participants =
        [
            new ParticipantSpecification
            {
                CountryId = ExampleIds.Countries.Italy, ActName = "Lucio Corsi", SongTitle = "Volevo essere un duro"
            }
        ],
        Group2Participants =
        [
            new ParticipantSpecification { CountryId = ExampleIds.Countries.Austria, ActName = "JJ", SongTitle = "Wasted Love" }
        ]
    };
}

public sealed record ParticipantSpecification
{
    public required Guid CountryId { get; init; }

    public required string ActName { get; init; }

    public required string SongTitle { get; init; }

    public void Deconstruct(out Guid countryId, out string actName, out string songTitle)
    {
        countryId = CountryId;
        actName = ActName;
        songTitle = SongTitle;
    }
}

internal static class CreateContest
{
    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPost("contests", HandleAsync)
            .WithName(EndpointIds.Contests.CreateContest)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest.")
            .HasApiVersion(1, 0)
            .Produces<CreateContestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreateContestResponse>>> HandleAsync(
        [FromBody] CreateContestRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static ErrorOr<Command> InitializeCommand(CreateContestRequest request) => new Command(request.ContestYear,
        request.CityName,
        request.ContestFormat,
        request.Group0CountryId,
        request.Group1Participants,
        request.Group2Participants);

    private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointIds.Contests.GetContest,
            new RouteValueDictionary { ["contestId"] = response.Contest.Id });

    private static ContestBuilder SpecifyFrom(this ContestBuilder builder, Command command)
    {
        var (contestYear,
            cityName,
            _,
            group0CountryId,
            group1Participants,
            group2Participants) = command;

        builder = builder.WithContestYear(ContestYear.FromValue(contestYear))
            .WithCityName(CityName.FromValue(cityName));

        if (group0CountryId is { } id)
        {
            builder = builder.AddGroup0Participant(CountryId.FromValue(id));
        }

        foreach (var (countryId, actName, songTitle) in group1Participants)
        {
            builder = builder.AddGroup1Participant(CountryId.FromValue(countryId),
                ActName.FromValue(actName),
                SongTitle.FromValue(songTitle));
        }

        foreach (var (countryId, actName, songTitle) in group2Participants)
        {
            builder = builder.AddGroup2Participant(CountryId.FromValue(countryId),
                ActName.FromValue(actName),
                SongTitle.FromValue(songTitle));
        }

        return builder;
    }

    internal sealed record Command(
        int ContestYear,
        string CityName,
        ContestFormat ContestFormat,
        Guid? Group0CountryId,
        ParticipantSpecification[] Group1Participants,
        ParticipantSpecification[] Group2Participants) : ICommand<CreateContestResponse>;

    internal sealed class Handler(AppDbContext dbContext, IContestIdGenerator idGenerator)
        : ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await InitializeBuilder(command.ContestFormat)
                .SpecifyFrom(command)
                .Build(idGenerator)
                .FailOnContestYearConflict(dbContext.Contests.AsNoTracking().AsSplitQuery())
                .FailOnOrphanParticipant(dbContext.Countries.AsNoTracking().AsSplitQuery())
                .ThenDo(contest => dbContext.Contests.Add(contest))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(contest => new CreateContestResponse(contest.ToContestDto()));

        private static ContestBuilder InitializeBuilder(ContestFormat contestFormat) => contestFormat switch
        {
            ContestFormat.Stockholm => Contest.CreateStockholmFormat(),
            ContestFormat.Liverpool => Contest.CreateLiverpoolFormat(),
            _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
        };
    }
}
