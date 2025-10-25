using Eurocentric.Apis.Admin.V1.Enums;
using ChildBroadcastDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.ChildBroadcast;
using ChildBroadcastEntity = Eurocentric.Domain.Aggregates.Contests.ChildBroadcast;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.Contest;
using GlobalTelevoteDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.GlobalTelevote;
using GlobalTelevoteEntity = Eurocentric.Domain.Aggregates.Contests.GlobalTelevote;
using ParticipantDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.Participant;
using ParticipantEntity = Eurocentric.Domain.Aggregates.Contests.Participant;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

public static class MappingExtensions
{
    public static ContestDto ToDto(this ContestAggregate aggregate)
    {
        return new ContestDto
        {
            Id = aggregate.Id.Value,
            ContestYear = aggregate.ContestYear.Value,
            CityName = aggregate.CityName.Value,
            ContestRules = aggregate.ContestRules.ToApiContestRules(),
            Queryable = aggregate.Queryable,
            GlobalTelevote = aggregate.GlobalTelevote?.ToDto(),
            ChildBroadcasts = aggregate.ChildBroadcasts.Select(MapToDto).ToArray(),
            Participants = aggregate.Participants.Select(MapToDto).ToArray(),
        };
    }

    private static ChildBroadcastDto MapToDto(ChildBroadcastEntity entity)
    {
        return new ChildBroadcastDto
        {
            ChildBroadcastId = entity.ChildBroadcastId.Value,
            ContestStage = entity.ContestStage.ToApiContestStage(),
            Completed = entity.Completed,
        };
    }

    private static GlobalTelevoteDto ToDto(this GlobalTelevoteEntity entity) =>
        new() { VotingCountryId = entity.VotingCountryId.Value };

    private static ParticipantDto MapToDto(ParticipantEntity entity)
    {
        return new ParticipantDto
        {
            ParticipatingCountryId = entity.ParticipatingCountryId.Value,
            SemiFinalDraw = entity.SemiFinalDraw.ToApiSemiFinalDraw(),
            ActName = entity.ActName.Value,
            SongTitle = entity.SongTitle.Value,
        };
    }
}
