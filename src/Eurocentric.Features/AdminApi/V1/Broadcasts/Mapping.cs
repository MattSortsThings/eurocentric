using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using DomainBroadcast = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using BroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Contracts.Broadcast;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

public static class Mapping
{
    public static BroadcastDto ToBroadcastDto(this DomainBroadcast broadcast) => new()
    {
        Id = broadcast.Id.Value,
        BroadcastDate = broadcast.BroadcastDate,
        ParentContestId = broadcast.ParentContestId.Value,
        ContestStage = (ContestStage)(int)broadcast.ContestStage,
        Completed = broadcast.Completed,
        Competitors = broadcast.Competitors.Select(competitor => new Competitor
        {
            CompetingCountryId = competitor.CompetingCountryId.Value,
            FinishingPosition = competitor.FinishingPosition,
            RunningOrderPosition = competitor.RunningOrderPosition,
            JuryAwards =
                competitor.JuryAwards.Select(award => new PointsAward
                {
                    VotingCountryId = award.VotingCountryId.Value, PointsValue = (int)award.PointsValue
                }).ToArray(),
            TelevoteAwards =
                competitor.TelevoteAwards.Select(award => new PointsAward
                {
                    VotingCountryId = award.VotingCountryId.Value, PointsValue = (int)award.PointsValue
                }).ToArray()
        }).ToArray(),
        Juries = broadcast.Juries.Select(voter => new Voter
        {
            VotingCountryId = voter.VotingCountryId.Value, PointsAwarded = voter.PointsAwarded
        }).ToArray(),
        Televotes = broadcast.Televotes.Select(voter => new Voter
        {
            VotingCountryId = voter.VotingCountryId.Value, PointsAwarded = voter.PointsAwarded
        }).ToArray()
    };
}
