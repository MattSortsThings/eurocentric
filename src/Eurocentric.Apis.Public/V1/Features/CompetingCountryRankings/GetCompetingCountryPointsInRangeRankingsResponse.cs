using Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;

namespace Eurocentric.Apis.Public.V1.Features.CompetingCountryRankings;

public sealed record GetCompetingCountryPointsInRangeRankingsResponse(
    CompetingCountryPointsInRangeRanking[] Rankings,
    CompetingCountryPointsInRangeMetadata Metadata
)
{
    public bool Equals(GetCompetingCountryPointsInRangeRankingsResponse? other)
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
