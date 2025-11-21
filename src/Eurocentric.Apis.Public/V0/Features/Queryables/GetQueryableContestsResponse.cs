using Eurocentric.Apis.Public.V0.Dtos.Queryables;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

public sealed record GetQueryableContestsResponse(QueryableContest[] QueryableContests)
{
    public bool Equals(GetQueryableContestsResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || QueryableContests.SequenceEqual(other.QueryableContests);
    }

    public override int GetHashCode() => QueryableContests.GetHashCode();
}
