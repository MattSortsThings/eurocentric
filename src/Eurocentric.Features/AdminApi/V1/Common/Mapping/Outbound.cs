using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using BroadcastAggregate = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using BroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V1.Common.Mapping;

internal static class Outbound
{
    internal static BroadcastDto ToBroadcastDto(this BroadcastAggregate broadcast) => new()
    {
        Id = broadcast.Id.Value,
        BroadcastDate = broadcast.BroadcastDate.Value,
        ParentContestId = broadcast.ParentContestId.Value,
        ContestStage = (ContestStage)(int)broadcast.ContestStage,
        Completed = broadcast.Completed,
        Competitors = broadcast.Competitors.Select(c => new Competitor
        {
            CompetingCountryId = c.CompetingCountryId.Value,
            RunningOrderPosition = c.RunningOrderPosition,
            FinishingPosition = c.FinishingPosition,
            JuryAwards =
                c.JuryAwards.Select(a =>
                    new Award { VotingCountryId = a.VotingCountryId.Value, PointsValue = (int)a.PointsValue }).ToArray(),
            TelevoteAwards =
                c.TelevoteAwards.Select(a =>
                    new Award { VotingCountryId = a.VotingCountryId.Value, PointsValue = (int)a.PointsValue }).ToArray()
        }).ToArray(),
        Juries = broadcast.Juries.Select(v => new Voter
        {
            VotingCountryId = v.VotingCountryId.Value, PointsAwarded = v.PointsAwarded
        }).ToArray(),
        Televotes = broadcast.Televotes.Select(v => new Voter
        {
            VotingCountryId = v.VotingCountryId.Value, PointsAwarded = v.PointsAwarded
        }).ToArray()
    };

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
