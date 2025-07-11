using System.Linq.Expressions;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using DomainContest = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Contracts.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

internal static class Projections
{
    internal static readonly Expression<Func<DomainContest, ContestDto>> ContestToContestDto = contest => new ContestDto
    {
        Id = contest.Id.Value,
        ContestYear = contest.ContestYear.Value,
        CityName = contest.CityName.Value,
        ContestFormat = (ContestFormat)(int)contest.ContestFormat,
        Completed = contest.Completed,
        ChildBroadcasts =
            contest.ChildBroadcasts.Select(memo => new BroadcastMemo
            {
                BroadcastId = memo.BroadcastId.Value,
                ContestStage = (ContestStage)(int)memo.ContestStage,
                BroadcastStatus = (BroadcastStatus)(int)memo.BroadcastStatus
            }).ToArray(),
        Participants = contest.Participants.Select(participant => new Participant
        {
            ParticipatingCountryId = participant.ParticipatingCountryId.Value,
            ParticipantGroup = (int)participant.ParticipantGroup,
            ActName = participant.ActName != null ? participant.ActName.Value : null,
            SongTitle = participant.SongTitle != null ? participant.SongTitle.Value : null
        }).ToArray()
    };
}
