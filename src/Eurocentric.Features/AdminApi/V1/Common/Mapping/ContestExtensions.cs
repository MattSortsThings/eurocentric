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
        ChildBroadcasts = contest.ChildBroadcasts.Select(MapToChildBroadcastDto).ToArray(),
        Participants = contest.Participants.Select(MapToParticipantDto).ToArray(),
        GlobalTelevote = contest.GlobalTelevote?.ToGlobalTelevoteDto()
    };


    private static ParticipantDto MapToParticipantDto(Participant participant) => new()
    {
        ParticipatingCountryId = participant.ParticipatingCountryId.Value,
        SemiFinalDraw = (SemiFinalDraw)(int)participant.SemiFinalDraw,
        ActName = participant.ActName.Value,
        SongTitle = participant.SongTitle.Value
    };

    private static ChildBroadcastDto MapToChildBroadcastDto(ChildBroadcast childBroadcast) => new()
    {
        BroadcastId = childBroadcast.BroadcastId.Value,
        ContestStage = (ContestStage)(int)childBroadcast.ContestStage,
        Completed = childBroadcast.Completed
    };

    private static GlobalTelevoteDto ToGlobalTelevoteDto(this GlobalTelevote televote) =>
        new() { ParticipatingCountryId = televote.ParticipatingCountryId.Value };
}
