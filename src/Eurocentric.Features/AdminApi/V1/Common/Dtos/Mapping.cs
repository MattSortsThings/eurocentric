using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

internal static class Mapping
{
    internal static Broadcast ToBroadcastDto(this Domain.Broadcasts.Broadcast broadcast) => new()
    {
        Id = broadcast.Id.Value,
        ContestId = broadcast.ContestId.Value,
        ContestStage = Enum.Parse<ContestStage>(broadcast.ContestStage.ToString()),
        Status = Enum.Parse<BroadcastStatus>(broadcast.Status.ToString()),
        Competitors = broadcast.Competitors.Select(competitor => competitor.ToCompetitorDto()).ToArray(),
        Juries = broadcast.Juries.Select(jury => jury.ToVoteDto()).ToArray(),
        Televotes = broadcast.Televotes.Select(televote => televote.ToVoteDto()).ToArray()
    };

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

    private static Competitor ToCompetitorDto(this Domain.Broadcasts.Competitor competitor) => new()
    {
        CompetingCountryId = competitor.CompetingCountryId.Value,
        RunningOrderPosition = competitor.RunningOrderPosition,
        FinishingPosition = competitor.FinishingPosition,
        JuryAwards = competitor.JuryAwards.Select(award => award.ToAwardDto()).ToArray(),
        TelevoteAwards = competitor.TelevoteAwards.Select(award => award.ToAwardDto()).ToArray()
    };

    private static Award ToAwardDto(this Domain.ValueObjects.Award award) =>
        new(award.VotingCountryId.Value, (int)award.PointsValue);

    private static Vote ToVoteDto(this Domain.Broadcasts.Vote vote) => new(vote.VotingCountryId.Value, vote.PointsAwarded);
}
