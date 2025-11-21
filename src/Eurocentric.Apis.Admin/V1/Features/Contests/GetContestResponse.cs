using Eurocentric.Apis.Admin.V1.Dtos.Contests;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record GetContestResponse(Contest Contest)
{
    public bool Equals(GetContestResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Contest.Equals(other.Contest);
    }

    public override int GetHashCode() => Contest.GetHashCode();
}
