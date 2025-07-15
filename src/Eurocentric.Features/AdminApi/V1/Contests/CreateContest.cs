using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
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
using Contest = Eurocentric.Features.AdminApi.V1.Common.Contracts.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record CreateContestRequest : IExampleProvider<CreateContestRequest>
{
    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public required int ContestYear { get; init; }

    /// <summary>
    ///     The name of the city in which the contest is held.
    /// </summary>
    public required string CityName { get; init; }

    /// <summary>
    ///     The contest's format.
    /// </summary>
    public required ContestFormat ContestFormat { get; init; }

    /// <summary>
    ///     The ID of the group 0 participating country, if present.
    /// </summary>
    public Guid? Group0ParticipatingCountryId { get; init; }

    /// <summary>
    ///     Specifications for the group 1 participants.
    /// </summary>
    public required ContestParticipantSpecification[] Group1Participants { get; init; }

    /// <summary>
    ///     Specifications for the group 2 participants.
    /// </summary>
    public required ContestParticipantSpecification[] Group2Participants { get; init; }

    public static CreateContestRequest CreateExample() => new()
    {
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        Group0ParticipatingCountryId = ExampleIds.CountryXx,
        Group1Participants =
        [
            new ContestParticipantSpecification(ExampleIds.CountryAt, "JJ", "Wasted Love")
        ],
        Group2Participants =
        [
            new ContestParticipantSpecification(ExampleIds.CountryIt, "Lucio Corsi", "Volevo Essere Un Duro")
        ]
    };
}

/// <summary>
///     Specifies a group 1 or group 2 contest participant.
/// </summary>
/// <param name="ParticipatingCountryId">The ID of the participating country.</param>
/// <param name="ActName">The participant's act name.</param>
/// <param name="SongTitle">The participant's song title.</param>
public sealed record ContestParticipantSpecification(Guid ParticipatingCountryId, string ActName, string SongTitle);

public sealed record CreateContestResponse(Contest Contest) : IExampleProvider<CreateContestResponse>
{
    public static CreateContestResponse CreateExample() => new(Contest.CreateExample() with { ChildBroadcasts = [] });
}

internal static class CreateContest
{
    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder v1Group)
    {
        v1Group.MapPost("contests", ExecuteAsync)
            .WithName(EndpointConstants.Names.Contests.CreateContest)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest.")
            .WithTags(EndpointConstants.Tags.Contests)
            .HasApiVersion(1, 0)
            .Produces<CreateContestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return v1Group;
    }

    private static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreateContestResponse>>> ExecuteAsync(
        [FromBody] CreateContestRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointConstants.Names.Contests.CreateContest,
            new RouteValueDictionary { { "contestId", response.Contest.Id } });

    private static ErrorOr<Command> InitializeCommand(CreateContestRequest requestBody) =>
        ErrorOrFactory.From(new Command(requestBody.ContestYear,
            requestBody.CityName,
            requestBody.ContestFormat,
            requestBody.Group0ParticipatingCountryId,
            requestBody.Group1Participants,
            requestBody.Group2Participants));

    private static ContestBuilder Apply(this ContestBuilder builder, Command command)
    {
        (int contestYear,
            string cityName,
            _,
            Guid? group0ParticipatingCountryId,
            ContestParticipantSpecification[] group1Participants,
            ContestParticipantSpecification[] group2Participants) = command;

        if (group0ParticipatingCountryId is { } countryId)
        {
            builder.AddGroup0Participant(CountryId.FromValue(countryId));
        }

        foreach (var (id, act, song) in group1Participants)
        {
            builder.AddGroup1Participant(CountryId.FromValue(id), ActName.FromValue(act), SongTitle.FromValue(song));
        }

        foreach (var (id, act, song) in group2Participants)
        {
            builder.AddGroup2Participant(CountryId.FromValue(id), ActName.FromValue(act), SongTitle.FromValue(song));
        }

        return builder.WithContestYear(ContestYear.FromValue(contestYear))
            .WithCityName(CityName.FromValue(cityName));
    }

    internal sealed record Command(
        int ContestYear,
        string CityName,
        ContestFormat ContestFormat,
        Guid? Group0ParticipatingCountryId,
        ContestParticipantSpecification[] Group1Participants,
        ContestParticipantSpecification[] Group2Participants) : ICommand<CreateContestResponse>;

    internal sealed class Handler(AppDbContext dbContext, IContestIdGenerator idGenerator) :
        ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return await InitializeContestBuilder(command.ContestFormat)
                .Apply(command)
                .Build(idGenerator.CreateSingle)
                .FailOnContestYearConflict(dbContext.Contests.AsNoTracking().AsSplitQuery())
                .FailOnOrphanParticipatingCountryId(dbContext.Countries.AsNoTracking().AsSplitQuery())
                .ThenDo(contest => dbContext.Contests.Add(contest))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(contest => new CreateContestResponse(contest.ToContestDto()));
        }

        private static ContestBuilder InitializeContestBuilder(ContestFormat contestFormat) => contestFormat switch
        {
            ContestFormat.Stockholm => StockholmFormatContest.Create(),
            ContestFormat.Liverpool => LiverpoolFormatContest.Create(),
            _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
        };
    }
}
