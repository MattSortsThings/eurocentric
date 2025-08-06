using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.CreateContest;

internal static class CreateContestFeature
{
    internal static async Task<IResult> ExecuteAsync([FromBody] CreateContestRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(requestBody.ToCommand(), MapToCreatedAtRoute, cancellationToken);

    private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse responseBody) =>
        TypedResults.CreatedAtRoute(responseBody,
            EndpointNames.Routes.Contests.GetContest,
            new RouteValueDictionary { { "contestId", responseBody.Contest.Id } });

    private static Command ToCommand(this CreateContestRequest requestBody) => new(requestBody.ContestYear,
        requestBody.CityName,
        requestBody.ContestFormat,
        requestBody.Group0ParticipatingCountryId,
        requestBody.Group1ParticipantData,
        requestBody.Group2ParticipantData);

    private static ContestBuilder Apply(this ContestBuilder builder, Command command)
    {
        builder = builder.WithContestYear(ContestYear.FromValue(command.ContestYear))
            .WithCityName(CityName.FromValue(command.CityName));

        if (command.Group0ParticipatingCountryId is { } g)
        {
            builder.AddGroup0Participant(CountryId.FromValue(g));
        }

        foreach (var (id, act, song) in command.Group1ParticipantData)
        {
            builder.AddGroup1Participant(CountryId.FromValue(id), ActName.FromValue(act), SongTitle.FromValue(song));
        }

        foreach (var (id, act, song) in command.Group2ParticipantData)
        {
            builder.AddGroup2Participant(CountryId.FromValue(id), ActName.FromValue(act), SongTitle.FromValue(song));
        }

        return builder;
    }

    internal sealed record Command(
        int ContestYear,
        string CityName,
        ContestFormat ContestFormat,
        Guid? Group0ParticipatingCountryId,
        ContestParticipantDatum[] Group1ParticipantData,
        ContestParticipantDatum[] Group2ParticipantData) : ICommand<CreateContestResponse>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext, IContestIdProvider idProvider)
        : ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetContestBuilder(command.ContestFormat)
                .Apply(command)
                .Build(idProvider.CreateSingle)
                .FailOnContestYearConflict(dbContext.Contests.AsNoTracking().AsSplitQuery())
                .FailOnParticipatingCountryNotFound(dbContext.Countries.AsNoTracking().AsSplitQuery())
                .ThenDo(contest => dbContext.Contests.Add(contest))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(contest => contest.ToContestDto())
                .Then(contest => new CreateContestResponse(contest));

        private static ContestBuilder GetContestBuilder(ContestFormat contestFormat) => contestFormat switch
        {
            ContestFormat.Liverpool => LiverpoolFormatContest.Create(),
            ContestFormat.Stockholm => StockholmFormatContest.Create(),
            _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
        };
    }
}
