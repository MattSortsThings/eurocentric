using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using ContestAggregate = Eurocentric.Domain.V0Entities.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V0.Common.Dtos.Contest;

namespace Eurocentric.Features.AdminApi.V0.Common.Mappings;

internal static class Outbound
{
    internal static ContestDto ToContestDto(this ContestAggregate contest) => new()
    {
        Id = contest.Id,
        ContestYear = contest.ContestYear,
        CityName = contest.CityName,
        ContestFormat = (ContestFormat)(int)contest.ContestFormat,
        Completed = contest.Completed,
        ChildBroadcasts =
            contest.ChildBroadcasts.Select(memo => new ChildBroadcast
            {
                BroadcastId = memo.BroadcastId,
                ContestStage = (ContestStage)(int)memo.ContestStage,
                Completed = memo.Completed
            }).ToArray(),
        Participants = contest.Participants.Select(participant => new Participant
        {
            ParticipatingCountryId = participant.ParticipatingCountryId,
            ParticipantGroup = (int)participant.ParticipantGroup,
            ActName = participant.ActName,
            SongTitle = participant.SongTitle
        }).ToArray()
    };
}
