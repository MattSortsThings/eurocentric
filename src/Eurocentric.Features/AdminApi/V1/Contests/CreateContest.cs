using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
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
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Contest = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using Participant = Eurocentric.Features.AdminApi.V1.Common.Dtos.Participant;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record CreateContestResponse(Contest Contest) : IExampleProvider<CreateContestResponse>
{
    public static CreateContestResponse CreateExample() => new(Contest.CreateExample() with
    {
        Status = ContestStatus.Initialized,
        BroadcastMemos = [],
        Participants =
        [
            new Participant { ParticipatingCountryId = ExampleValues.CountryId3Of3, Group = 0 },
            new Participant
            {
                ParticipatingCountryId = ExampleValues.CountryId2Of3,
                Group = 1,
                ActName = "Lucio Corsi",
                SongTitle = "Volevo Essere Un Duro"
            },
            Participant.CreateExample()
        ]
    });
}

public sealed record CreateContestRequest : IExampleProvider<CreateContestRequest>
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public Guid? Group0CountryId { get; init; }

    public required ContestParticipantDatum[] Group1Participants { get; init; }

    public required ContestParticipantDatum[] Group2Participants { get; init; }

    public static CreateContestRequest CreateExample() => new()
    {
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        Group0CountryId = ExampleValues.CountryId3Of3,
        Group1Participants =
        [
            new ContestParticipantDatum
            {
                CountryId = ExampleValues.CountryId2Of3, ActName = "Lucio Corsi", SongTitle = "Volevo Essere Un Duro"
            }
        ],
        Group2Participants =
        [
            new ContestParticipantDatum { CountryId = ExampleValues.CountryId1Of3, ActName = "JJ", SongTitle = "Wasted Love" }
        ]
    };
}

public sealed record ContestParticipantDatum
{
    public required Guid CountryId { get; init; }

    public required string ActName { get; init; }

    public required string SongTitle { get; init; }

    internal void Deconstruct(out Guid countryId, out string actName, out string songTitle)
    {
        countryId = CountryId;
        actName = ActName;
        songTitle = SongTitle;
    }
}

internal static class CreateContest
{
    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapPost("contests", Endpoint.HandleAsync)
            .WithName(RouteIds.Contests.CreateContest)
            .HasApiVersion(1, 0)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest.")
            .WithTags(EndpointTags.Contests)
            .Produces<CreateContestResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return apiVersionGroup;
    }


    private static Action<ContestBuilder> ToContestBuilderMutator(this Command command)
    {
        var (contestYear, cityName, _, group0CountryId, group1Participants, group2Participants) = command;

        Action<ContestBuilder> mutator = builder => { };

        mutator += builder => builder.WithYear(ContestYear.FromValue(contestYear));

        mutator += builder => builder.WithCityName(CityName.FromValue(cityName));

        if (group0CountryId.HasValue)
        {
            mutator += builder => builder.WithGroupZeroParticipant(CountryId.FromValue(group0CountryId.Value));
        }

        foreach (var (countryId, actName, songTitle) in group1Participants)
        {
            mutator += builder => builder.WithGroupOneParticipant(CountryId.FromValue(countryId),
                ActName.FromValue(actName),
                SongTitle.FromValue(songTitle));
        }

        foreach (var (countryId, actName, songTitle) in group2Participants)
        {
            mutator += builder => builder.WithGroupTwoParticipant(CountryId.FromValue(countryId),
                ActName.FromValue(actName),
                SongTitle.FromValue(songTitle));
        }

        return mutator;
    }

    internal sealed record Command(
        int ContestYear,
        string CityName,
        ContestFormat ContestFormat,
        Guid? Group0CountryId,
        ContestParticipantDatum[] Group1Participants,
        ContestParticipantDatum[] Group2Participants) : ICommand<CreateContestResponse>;

    internal sealed class Handler(AppDbContext dbContext, IContestIdProvider idProvider) :
        ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await InitializeContestBuilder(command.ContestFormat)
                .ThenDo(builder => command.ToContestBuilderMutator().Invoke(builder))
                .Then(builder => builder.Build(idProvider.Create))
                .FailIfContestYearIsNotUnique(dbContext.Contests.AsNoTracking())
                .FailIfOrphanParticipatingCountryIds(dbContext.Countries.AsNoTracking())
                .ThenDo(contest => dbContext.Contests.Add(contest))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(contest => new CreateContestResponse(contest.ToContestDto()));

        private static ErrorOr<ContestBuilder> InitializeContestBuilder(ContestFormat contestFormat) => contestFormat switch
        {
            ContestFormat.Liverpool => LiverpoolFormatContest.Create(),
            ContestFormat.Stockholm => StockholmFormatContest.Create(),
            _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
        };
    }

    private static class Endpoint
    {
        internal static async Task<Results<CreatedAtRoute<CreateContestResponse>, ProblemHttpResult>> HandleAsync(
            [FromBody] CreateContestRequest request,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeCommand(request)
            .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(MapToCreatedAtRoute);

        private static ErrorOr<Command> InitializeCommand(CreateContestRequest request) =>
            ErrorOrFactory.From(new Command(request.ContestYear,
                request.CityName,
                request.ContestFormat,
                request.Group0CountryId,
                request.Group1Participants,
                request.Group2Participants));

        private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse response) =>
            TypedResults.CreatedAtRoute(response,
                RouteIds.Contests.GetContest,
                new RouteValueDictionary { { "contestId", response.Contest.Id } });
    }
}
