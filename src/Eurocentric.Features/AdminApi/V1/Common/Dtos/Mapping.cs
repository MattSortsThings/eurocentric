using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

internal static class Mapping
{
    internal static Contest ToContestDto(this Domain.Contests.Contest contest) => new()
    {
        Id = contest.Id.Value,
        Year = contest.Year.Value,
        CityName = contest.CityName.Value,
        Format = Enum.Parse<ContestFormat>(contest.Format.ToString()),
        Status = Enum.Parse<ContestStatus>(contest.Status.ToString()),
        BroadcastMemos = contest.BroadcastMemos.Select(memo => memo.ToBroadcastMemoDto()).ToArray(),
        Participants = contest.Participants.Select(memo => memo.ToParticipantDto()).ToArray()
    };

    internal static Country ToCountryDto(this Domain.Countries.Country country) => new()
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        Name = country.Name.Value,
        ContestMemos = country.ContestMemos.Select(memo => memo.ToContestMemoDto()).ToArray()
    };

    private static BroadcastMemo ToBroadcastMemoDto(this Domain.ValueObjects.BroadcastMemo broadcastMemo) =>
        new(broadcastMemo.BroadcastId.Value, Enum.Parse<ContestStage>(broadcastMemo.ContestStage.ToString()),
            Enum.Parse<BroadcastStatus>(broadcastMemo.Status.ToString()));

    private static ContestMemo ToContestMemoDto(this Domain.ValueObjects.ContestMemo contestMemo) =>
        new(contestMemo.ContestId.Value,
            Enum.Parse<ContestStatus>(contestMemo.Status.ToString()));

    private static Participant ToParticipantDto(this Domain.Contests.Participant participant) => new()
    {
        ParticipatingCountryId = participant.ParticipatingCountryId.Value,
        Group = (int)participant.Group,
        ActName = participant.ActName?.Value,
        SongTitle = participant.SongTitle?.Value
    };
}
