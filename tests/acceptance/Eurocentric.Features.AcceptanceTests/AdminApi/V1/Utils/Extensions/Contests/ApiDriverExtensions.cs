using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ParticipantDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Participant;
using ChildBroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.ChildBroadcast;
using ContestFormat = Eurocentric.Features.AdminApi.V1.Common.Enums.ContestFormat;
using ContestStage = Eurocentric.Features.AdminApi.V1.Common.Enums.ContestStage;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;

public static class ApiDriverExtensions
{
    public static async Task<ContestDto> CreateSingleLiverpoolFormatContestAsync(this IApiDriver driver,
        Guid[] group2CountryIds = null!,
        Guid[] group1CountryIds = null!,
        Guid group0CountryId = default,
        string cityName = TestDefaults.CityName,
        int contestYear = 0)
    {
        List<Participant> participants = group1CountryIds.ToGroup1Participants()
            .Concat(group2CountryIds.ToGroup2Participants())
            .Prepend(group0CountryId.ToGroup0Participant())
            .ToList();

        ContestYear year = ContestYear.FromValue(contestYear).Value;
        CityName city = CityName.FromValue(cityName).Value;
        ContestId id = ContestId.FromValue(Guid.NewGuid());

        LiverpoolFormatContest contest = new(id, year, city, participants);

        await driver.BackDoor.ExecuteScopedAsync(PersistContestAsync(contest));

        return contest.ToContestDto();
    }

    public static async Task<ContestDto> CreateSingleStockholmFormatContestAsync(this IApiDriver driver,
        Guid[] group2CountryIds = null!,
        Guid[] group1CountryIds = null!,
        string cityName = TestDefaults.CityName,
        int contestYear = 0)
    {
        List<Participant> participants = group1CountryIds.ToGroup1Participants()
            .Concat(group2CountryIds.ToGroup2Participants())
            .ToList();

        ContestYear year = ContestYear.FromValue(contestYear).Value;
        CityName city = CityName.FromValue(cityName).Value;
        ContestId id = ContestId.FromValue(Guid.NewGuid());

        StockholmFormatContest contest = new(id, year, city, participants);

        await driver.BackDoor.ExecuteScopedAsync(PersistContestAsync(contest));

        return contest.ToContestDto();
    }

    public static async Task DeleteSingleContestAsync(this IApiDriver driver, Guid contestId)
    {
        ContestId contestIdToDelete = ContestId.FromValue(contestId);

        await driver.BackDoor.ExecuteScopedAsync(DeleteContestAsync(contestIdToDelete));
    }

    private static Func<IServiceProvider, Task> PersistContestAsync(ContestAggregate contest)
    {
        ContestAggregate contestToAdd = contest;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            dbContext.Contests.Add(contestToAdd);
            await dbContext.SaveChangesAsync();
        };
    }

    private static Func<IServiceProvider, Task> DeleteContestAsync(ContestId contestId)
    {
        ContestId idToDelete = contestId;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            await dbContext.Contests.Where(c => c.Id == idToDelete).ExecuteDeleteAsync();
        };
    }

    private static IEnumerable<Participant> ToGroup2Participants(this IEnumerable<Guid> countryIds) => countryIds.Select(id =>
        new Participant(CountryId.FromValue(id),
            ParticipantGroup.Two,
            ActName.FromValue(TestDefaults.ActName).Value,
            SongTitle.FromValue(TestDefaults.SongTitle).Value));

    private static IEnumerable<Participant> ToGroup1Participants(this IEnumerable<Guid> countryIds) => countryIds.Select(id =>
        new Participant(CountryId.FromValue(id),
            ParticipantGroup.One,
            ActName.FromValue(TestDefaults.ActName).Value,
            SongTitle.FromValue(TestDefaults.SongTitle).Value));

    private static Participant ToGroup0Participant(this Guid countryId) => new(CountryId.FromValue(countryId));

    internal static ContestDto ToContestDto(this ContestAggregate contest) => new()
    {
        Id = contest.Id.Value,
        ContestYear = contest.ContestYear.Value,
        CityName = contest.CityName.Value,
        ContestFormat = (ContestFormat)(int)contest.ContestFormat,
        Completed = contest.Completed,
        ChildBroadcasts =
            contest.ChildBroadcasts.Select(broadcast => new ChildBroadcastDto
            {
                BroadcastId = broadcast.BroadcastId.Value,
                ContestStage = (ContestStage)(int)broadcast.ContestStage,
                Completed = contest.Completed
            }).ToArray(),
        Participants = contest.Participants.Select(participant => new ParticipantDto
        {
            ParticipatingCountryId = participant.ParticipatingCountryId.Value,
            ParticipantGroup = (int)participant.ParticipantGroup,
            ActName = participant.ActName?.Value ?? null,
            SongTitle = participant.SongTitle?.Value ?? null
        }).ToArray()
    };
}
