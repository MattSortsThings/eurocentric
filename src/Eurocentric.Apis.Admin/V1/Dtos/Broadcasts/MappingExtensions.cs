using Eurocentric.Apis.Admin.V1.Enums;
using BroadcastAggregate = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using BroadcastDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Broadcast;
using CompetitorDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Competitor;
using CompetitorEntity = Eurocentric.Domain.Aggregates.Broadcasts.Competitor;
using JuryAwardDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.JuryAward;
using JuryAwardValueObject = Eurocentric.Domain.ValueObjects.JuryAward;
using JuryDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Jury;
using JuryEntity = Eurocentric.Domain.Aggregates.Broadcasts.Jury;
using TelevoteAwardDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.TelevoteAward;
using TelevoteAwardValueObject = Eurocentric.Domain.ValueObjects.TelevoteAward;
using TelevoteDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Televote;
using TelevoteEntity = Eurocentric.Domain.Aggregates.Broadcasts.Televote;

namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

public static class MappingExtensions
{
    public static BroadcastDto ToDto(this BroadcastAggregate aggregate)
    {
        return new BroadcastDto
        {
            Id = aggregate.Id.Value,
            BroadcastDate = aggregate.BroadcastDate.Value,
            ParentContestId = aggregate.ParentContestId.Value,
            ContestStage = aggregate.ContestStage.ToApiContestStage(),
            Completed = aggregate.Completed,
            Competitors = aggregate.Competitors.Select(MapToDto).ToArray(),
            Juries = aggregate.Juries.Select(MapToDto).ToArray(),
            Televotes = aggregate.Televotes.Select(MapToDto).ToArray(),
        };
    }

    private static CompetitorDto MapToDto(CompetitorEntity entity)
    {
        return new CompetitorDto
        {
            CompetingCountryId = entity.CompetingCountryId.Value,
            RunningOrderSpot = entity.RunningOrderSpot.Value,
            FinishingPosition = entity.FinishingPosition.Value,
            JuryAwards = entity.JuryAwards.Select(MapToDto).ToArray(),
            TelevoteAwards = entity.TelevoteAwards.Select(MapToDto).ToArray(),
        };
    }

    private static JuryDto MapToDto(JuryEntity entity) =>
        new() { VotingCountryId = entity.VotingCountryId.Value, PointsAwarded = entity.PointsAwarded };

    private static TelevoteDto MapToDto(TelevoteEntity entity) =>
        new() { VotingCountryId = entity.VotingCountryId.Value, PointsAwarded = entity.PointsAwarded };

    private static JuryAwardDto MapToDto(JuryAwardValueObject valueObject) =>
        new() { VotingCountryId = valueObject.VotingCountryId.Value, PointsValue = (int)valueObject.PointsValue };

    private static TelevoteAwardDto MapToDto(TelevoteAwardValueObject valueObject) =>
        new() { VotingCountryId = valueObject.VotingCountryId.Value, PointsValue = (int)valueObject.PointsValue };
}
