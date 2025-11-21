using Eurocentric.Apis.Public.V1.Dtos.Listings;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetCompetingCountryPointsListingsResponse(
    CompetingCountryJuryPointsListing[] JuryPointsListings,
    CompetingCountryTelevotePointsListing[] TelevotePointsListings,
    CompetingCountryPointsMetadata Metadata
)
{
    public bool Equals(GetCompetingCountryPointsListingsResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return JuryPointsListings.SequenceEqual(other.JuryPointsListings)
            && TelevotePointsListings.SequenceEqual(other.TelevotePointsListings)
            && Metadata.Equals(other.Metadata);
    }

    public override int GetHashCode() => HashCode.Combine(JuryPointsListings, TelevotePointsListings, Metadata);
}
