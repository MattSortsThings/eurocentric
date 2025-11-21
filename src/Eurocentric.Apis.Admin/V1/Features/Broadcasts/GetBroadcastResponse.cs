using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

public sealed record GetBroadcastResponse(Broadcast Broadcast)
{
    public bool Equals(GetBroadcastResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Broadcast.Equals(other.Broadcast);
    }

    public override int GetHashCode() => Broadcast.GetHashCode();
}
