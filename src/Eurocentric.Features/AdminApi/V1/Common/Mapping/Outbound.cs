using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V1.Common.Mapping;

internal static class Outbound
{
    internal static ContestDto ToContestDto(this ContestAggregate contest) => new()
    {
        Id = contest.Id.Value,
        ContestYear = contest.ContestYear.Value,
        CityName = contest.CityName.Value,
        ContestFormat = (ContestFormat)(int)contest.ContestFormat,
        Completed = contest.Completed,
        ChildBroadcasts =
            contest.ChildBroadcasts.Select(broadcast => new ChildBroadcast
            {
                BroadcastId = broadcast.BroadcastId.Value,
                ContestStage = (ContestStage)(int)broadcast.ContestStage,
                Completed = contest.Completed
            }).ToArray(),
        Participants = contest.Participants.Select(participant => new Participant
        {
            ParticipatingCountryId = participant.ParticipatingCountryId.Value,
            ParticipantGroup = (int)participant.ParticipantGroup,
            ActName = participant.ActName?.Value ?? null,
            SongTitle = participant.SongTitle?.Value ?? null
        }).ToArray()
    };

    internal static CountryDto ToCountryDto(this CountryAggregate country) => new()
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        CountryName = country.CountryName.Value,
        ParticipatingContestIds = country.ParticipatingContestIds.Select(id => id.Value).ToArray()
    };
}
