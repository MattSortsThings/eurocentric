using Eurocentric.Apis.Public.V1.Dtos.Listings;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetCompetingCountryResultListingsResponse(
    CompetingCountryResultListing[] Listings,
    CompetingCountryResultMetadata Metadata
)
{
    public bool Equals(GetCompetingCountryResultListingsResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Listings.SequenceEqual(other.Listings) && Metadata.Equals(other.Metadata);
    }

    public override int GetHashCode() => HashCode.Combine(Listings, Metadata);
}
