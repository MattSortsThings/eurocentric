using Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

namespace Eurocentric.Apis.Public.V1.Features.CompetitorRankings;

public sealed record GetCompetitorPointsInRangeRankingsResponse(
    CompetitorPointsInRangeRanking[] Rankings,
    CompetitorPointsInRangeMetadata Metadata
)
{
    public bool Equals(GetCompetitorPointsInRangeRankingsResponse? other)
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
