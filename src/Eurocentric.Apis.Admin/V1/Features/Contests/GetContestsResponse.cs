using Eurocentric.Apis.Admin.V1.Dtos.Contests;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record GetContestsResponse(Contest[] Contests)
{
    public bool Equals(GetContestsResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Contests.SequenceEqual(other.Contests);
    }

    public override int GetHashCode() => Contests.GetHashCode();
}
