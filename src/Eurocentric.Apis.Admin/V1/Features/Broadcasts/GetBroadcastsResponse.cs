using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

public sealed record GetBroadcastsResponse(Broadcast[] Broadcasts)
{
    public bool Equals(GetBroadcastsResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Broadcasts.SequenceEqual(other.Broadcasts);
    }

    public override int GetHashCode() => Broadcasts.GetHashCode();
}
