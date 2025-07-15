using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using DomainContest = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Contracts.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

internal static class Mappings
{
    internal static ContestDto ToContestDto(this DomainContest contest) => new()
    {
        Id = contest.Id.Value,
        ContestYear = contest.ContestYear.Value,
        CityName = contest.CityName.Value,
        Completed = contest.Completed,
        ContestFormat = (ContestFormat)(int)contest.ContestFormat,
        ChildBroadcasts =
            contest.ChildBroadcasts.Select(memo => new BroadcastMemo
            {
                BroadcastId = memo.BroadcastId.Value,
                ContestStage = (ContestStage)(int)memo.ContestStage,
                Completed = memo.Completed
            }).ToArray(),
        Participants = contest.Participants.Select(participant => new Participant
        {
            ParticipatingCountryId = participant.ParticipatingCountryId.Value,
            ParticipantGroup = (int)participant.ParticipantGroup,
            ActName = participant.ActName == null ? null : participant.ActName.Value,
            SongTitle = participant.SongTitle == null ? null : participant.SongTitle.Value
        }).ToArray()
    };
}
