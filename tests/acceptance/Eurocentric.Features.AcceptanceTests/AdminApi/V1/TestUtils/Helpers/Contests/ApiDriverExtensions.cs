using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.ValueObjects;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using ChildBroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.ChildBroadcast;
using GlobalTelevoteDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.GlobalTelevote;
using ParticipantDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Participant;
using SemiFinalDraw = Eurocentric.Domain.Enums.SemiFinalDraw;
using Participant = Eurocentric.Domain.Aggregates.Contests.Participant;
using GlobalTelevote = Eurocentric.Domain.Aggregates.Contests.GlobalTelevote;
using Contest = Eurocentric.Domain.Aggregates.Contests.Contest;
using ApiContestStage = Eurocentric.Features.AdminApi.V1.Common.Enums.ContestStage;
using ApiContestFormat = Eurocentric.Features.AdminApi.V1.Common.Enums.ContestFormat;
using ApiSemiFinalDraw = Eurocentric.Features.AdminApi.V1.Common.Enums.SemiFinalDraw;
using ChildBroadcast = Eurocentric.Domain.Aggregates.Contests.ChildBroadcast;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Contests;

public static class ApiDriverExtensions
{
    public static async Task DeleteSingleContestAsync(this IApiDriver apiDriver, Guid contestId)
    {
        ContestId contestIdToDelete = ContestId.FromValue(contestId);

        await apiDriver.BackDoor.ExecuteScopedAsync(BackdoorOperations.DeleteContestAsync(contestIdToDelete));
    }

    public static async Task<ContestDto> CreateSingleLiverpoolFormatContestAsync(
        this IApiDriver apiDriver,
        Guid globalTelevoteCountryId = default,
        Guid[] group2CountryIds = null!,
        Guid[] group1CountryIds = null!,
        string cityName = TestDefaults.CityName,
        int contestYear = 0)
    {
        ContestId id = ContestId.FromValue(Guid.NewGuid());
        ContestYear year = ContestYear.FromValue(contestYear).Value;
        CityName city = CityName.FromValue(cityName).Value;
        GlobalTelevote globalTelevote = new(CountryId.FromValue(globalTelevoteCountryId));

        List<Participant> participants = group1CountryIds.Select(MapToSemiFinal1Participant)
            .Concat(group2CountryIds.Select(MapToSemiFinal2Participant))
            .ToList();

        LiverpoolFormatContest contest = new(id, year, city, participants, globalTelevote);

        await apiDriver.BackDoor.ExecuteScopedAsync(BackdoorOperations.PersistContestAsync(contest));

        return contest.ToContestDto();
    }

    public static async Task<ContestDto> CreateSingleStockholmFormatContestAsync(
        this IApiDriver apiDriver,
        Guid[] group2CountryIds = null!,
        Guid[] group1CountryIds = null!,
        string cityName = TestDefaults.CityName,
        int contestYear = 0)
    {
        ContestId id = ContestId.FromValue(Guid.NewGuid());
        ContestYear year = ContestYear.FromValue(contestYear).Value;
        CityName city = CityName.FromValue(cityName).Value;

        List<Participant> participants = group1CountryIds.Select(MapToSemiFinal1Participant)
            .Concat(group2CountryIds.Select(MapToSemiFinal2Participant))
            .ToList();

        StockholmFormatContest contest = new(id, year, city, participants);

        await apiDriver.BackDoor.ExecuteScopedAsync(BackdoorOperations.PersistContestAsync(contest));

        return contest.ToContestDto();
    }

    private static Participant MapToSemiFinal1Participant(Guid countryId)
    {
        CountryId participatingCountryId = CountryId.FromValue(countryId);
        ActName actName = ActName.FromValue(TestDefaults.ActName).Value;
        SongTitle songTitle = SongTitle.FromValue(TestDefaults.SongTitle).Value;

        return new Participant(participatingCountryId, SemiFinalDraw.SemiFinal1, actName, songTitle);
    }

    private static Participant MapToSemiFinal2Participant(Guid countryId)
    {
        CountryId participatingCountryId = CountryId.FromValue(countryId);
        ActName actName = ActName.FromValue(TestDefaults.ActName).Value;
        SongTitle songTitle = SongTitle.FromValue(TestDefaults.SongTitle).Value;

        return new Participant(participatingCountryId, SemiFinalDraw.SemiFinal2, actName, songTitle);
    }

    private static ContestDto ToContestDto(this Contest contest) => new()
    {
        Id = contest.Id.Value,
        ContestYear = contest.ContestYear.Value,
        CityName = contest.CityName.Value,
        ContestFormat = (ApiContestFormat)(int)contest.ContestFormat,
        Completed = contest.Completed,
        ChildBroadcasts = contest.ChildBroadcasts.Select(MapToChildBroadcastDto).ToArray(),
        Participants = contest.Participants.Select(MapToParticipantDto).ToArray(),
        GlobalTelevote = contest.GlobalTelevote?.ToGlobalTelevoteDto()
    };

    private static ParticipantDto MapToParticipantDto(Participant participant) => new()
    {
        ParticipatingCountryId = participant.ParticipatingCountryId.Value,
        SemiFinalDraw = (ApiSemiFinalDraw)(int)participant.SemiFinalDraw,
        ActName = participant.ActName.Value,
        SongTitle = participant.SongTitle.Value
    };

    private static ChildBroadcastDto MapToChildBroadcastDto(ChildBroadcast childBroadcast) => new()
    {
        BroadcastId = childBroadcast.BroadcastId.Value,
        ContestStage = (ApiContestStage)(int)childBroadcast.ContestStage,
        Completed = childBroadcast.Completed
    };

    private static GlobalTelevoteDto ToGlobalTelevoteDto(this GlobalTelevote televote) =>
        new() { ParticipatingCountryId = televote.ParticipatingCountryId.Value };
}
