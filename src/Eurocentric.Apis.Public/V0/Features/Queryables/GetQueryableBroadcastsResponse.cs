using Eurocentric.Apis.Public.V0.Dtos.Queryables;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

public sealed record GetQueryableBroadcastsResponse(QueryableBroadcast[] QueryableBroadcasts)
{
    public bool Equals(GetQueryableBroadcastsResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || QueryableBroadcasts.SequenceEqual(other.QueryableBroadcasts);
    }

    public override int GetHashCode() => QueryableBroadcasts.GetHashCode();
}
