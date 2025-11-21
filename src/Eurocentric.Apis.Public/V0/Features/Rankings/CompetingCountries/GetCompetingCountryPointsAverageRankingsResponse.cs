using Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries;

namespace Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;

public sealed record GetCompetingCountryPointsAverageRankingsResponse(
    CompetingCountryPointsAverageRanking[] Rankings,
    CompetingCountryPointsAverageMetadata Metadata
)
{
    public bool Equals(GetCompetingCountryPointsAverageRankingsResponse? other)
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
