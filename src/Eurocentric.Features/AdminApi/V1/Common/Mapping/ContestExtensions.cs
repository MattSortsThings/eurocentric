using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Contest = Eurocentric.Domain.Aggregates.Contests.Contest;
using Participant = Eurocentric.Domain.Aggregates.Contests.Participant;
using ChildBroadcast = Eurocentric.Domain.Aggregates.Contests.ChildBroadcast;
using GlobalTelevote = Eurocentric.Domain.Aggregates.Contests.GlobalTelevote;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using ParticipantDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Participant;
using ChildBroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.ChildBroadcast;
using GlobalTelevoteDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.GlobalTelevote;

namespace Eurocentric.Features.AdminApi.V1.Common.Mapping;

internal static class ContestExtensions
{
    internal static ContestDto ToContestDto(this Contest contest) => new()
    {
        Id = contest.Id.Value,
        ContestYear = contest.ContestYear.Value,
        CityName = contest.CityName.Value,
        ContestFormat = (ContestFormat)(int)contest.ContestFormat,
        Completed = contest.Completed,
        ChildBroadcasts = contest.ChildBroadcasts.Select(broadcast => broadcast.ToChildBroadcastDto()).ToArray(),
        Participants = contest.Participants.Select(participant => participant.ToParticipantDto()).ToArray(),
        GlobalTelevote = contest.GlobalTelevote?.ToGlobalTelevoteDto()
    };


    internal static ParticipantDto ToParticipantDto(this Participant participant) => new()
    {
        ParticipatingCountryId = participant.ParticipatingCountryId.Value,
        SemiFinalDraw = (SemiFinalDraw)(int)participant.SemiFinalDraw,
        ActName = participant.ActName.Value,
        SongTitle = participant.SongTitle.Value
    };

    internal static ChildBroadcastDto ToChildBroadcastDto(this ChildBroadcast childBroadcast) => new()
    {
        BroadcastId = childBroadcast.BroadcastId.Value,
        ContestStage = (ContestStage)(int)childBroadcast.ContestStage,
        Completed = childBroadcast.Completed
    };

    internal static GlobalTelevoteDto ToGlobalTelevoteDto(this GlobalTelevote televote) =>
        new() { ParticipatingCountryId = televote.ParticipatingCountryId.Value };
}
