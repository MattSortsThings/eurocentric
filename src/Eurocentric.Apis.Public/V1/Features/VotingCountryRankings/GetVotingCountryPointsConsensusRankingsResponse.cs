using Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

namespace Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;

public sealed record GetVotingCountryPointsConsensusRankingsResponse(
    VotingCountryPointsConsensusRanking[] Rankings,
    VotingCountryPointsConsensusMetadata Metadata
)
{
    public bool Equals(GetVotingCountryPointsConsensusRankingsResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Rankings.SequenceEqual(other.Rankings) && Metadata.Equals(other.Metadata);
    }

    public override int GetHashCode() => HashCode.Combine(Rankings, Metadata);
}
