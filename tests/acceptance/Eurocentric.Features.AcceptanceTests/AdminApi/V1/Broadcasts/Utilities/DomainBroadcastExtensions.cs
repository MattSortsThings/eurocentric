using Eurocentric.Features.AdminApi.V1.Common.Enums;
using DomainBroadcast = Eurocentric.Domain.Broadcasts.Broadcast;
using BroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;
using DomainCompetitor = Eurocentric.Domain.Broadcasts.Competitor;
using CompetitorDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Competitor;
using DomainVoter = Eurocentric.Domain.Broadcasts.Voter;
using VoterDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Voter;
using DomainAward = Eurocentric.Domain.ValueObjects.Award;
using AwardDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Award;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.Utilities;

internal static class DomainBroadcastExtensions
{
    internal static BroadcastDto ToBroadcastDto(this DomainBroadcast broadcast) => new()
    {
        Id = broadcast.Id.Value,
        BroadcastDate = broadcast.BroadcastDate.Value,
        ParentContestId = broadcast.ParentContestId.Value,
        ContestStage = Enum.Parse<ContestStage>(broadcast.ContestStage.ToString()),
        BroadcastStatus = Enum.Parse<BroadcastStatus>(broadcast.BroadcastStatus.ToString()),
        Competitors = broadcast.Competitors.Select(competitor => competitor.ToCompetitorDto()).ToArray(),
        Juries = broadcast.Juries.Select(jury => jury.ToVoterDto()).ToArray(),
        Televotes = broadcast.Televotes.Select(televote => televote.ToVoterDto()).ToArray()
    };

    private static CompetitorDto ToCompetitorDto(this DomainCompetitor competitor) => new()
    {
        CompetingCountryId = competitor.CompetingCountryId.Value,
        FinishingPosition = competitor.FinishingPosition,
        RunningOrderPosition = competitor.RunningOrderPosition,
        JuryAwards = competitor.JuryAwards.Select(award => award.ToAwardDto()).ToArray(),
        TelevoteAwards = competitor.TelevoteAwards.Select(award => award.ToAwardDto()).ToArray()
    };

    private static VoterDto ToVoterDto(this DomainVoter voter) => new(voter.VotingCountryId.Value, voter.PointsAwarded);

    private static AwardDto ToAwardDto(this DomainAward award) => new(award.VotingCountryId.Value, (int)award.PointsValue);
}
