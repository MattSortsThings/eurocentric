using Eurocentric.Features.AdminApi.V1.Common.Enums;
using BroadcastMemoDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.BroadcastMemo;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using ParticipantDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Participant;
using DomainBroadcastMemo = Eurocentric.Domain.ValueObjects.BroadcastMemo;
using DomainContest = Eurocentric.Domain.Contests.Contest;
using DomainParticipant = Eurocentric.Domain.Contests.Participant;

namespace Eurocentric.Features.AdminApi.V1.Common.DomainMapping;

internal static class DomainContestExtensions
{
    internal static ContestDto ToContestDto(this DomainContest contest) => new()
    {
        Id = contest.Id.Value,
        ContestYear = contest.ContestYear.Value,
        CityName = contest.CityName.Value,
        ContestFormat = Enum.Parse<ContestFormat>(contest.ContestFormat.ToString()),
        ContestStatus = Enum.Parse<ContestStatus>(contest.ContestStatus.ToString()),
        ChildBroadcasts = contest.ChildBroadcasts.Select(memo => memo.ToBroadcastMemoDto()).ToArray(),
        Participants = contest.Participants.Select(participant => participant.ToParticipantDto()).ToArray()
    };

    private static ParticipantDto ToParticipantDto(this DomainParticipant participant) => new()
    {
        ParticipatingCountryId = participant.ParticipatingCountryId.Value,
        ParticipantGroup = (int)participant.ParticipantGroup,
        ActName = participant.ActName?.Value,
        SongTitle = participant.SongTitle?.Value
    };

    private static BroadcastMemoDto ToBroadcastMemoDto(this DomainBroadcastMemo broadcastMemo) => new(
        broadcastMemo.BroadcastId.Value,
        Enum.Parse<ContestStage>(broadcastMemo.ContestStage.ToString()),
        Enum.Parse<BroadcastStatus>(broadcastMemo.BroadcastStatus.ToString()));
}
