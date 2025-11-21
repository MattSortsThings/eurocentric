using Eurocentric.Apis.Public.V0.Dtos.Listings;

namespace Eurocentric.Apis.Public.V0.Features.Listings;

public sealed record GetBroadcastResultListingsResponse(
    BroadcastResultListing[] Listings,
    BroadcastResultMetadata Metadata
)
{
    public bool Equals(GetBroadcastResultListingsResponse? other)
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
