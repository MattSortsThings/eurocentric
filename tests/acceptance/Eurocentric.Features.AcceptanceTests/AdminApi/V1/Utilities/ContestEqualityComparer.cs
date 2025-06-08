using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

internal sealed class ContestEqualityComparer : IEqualityComparer<Contest>
{
    public bool Equals(Contest? x, Contest? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Id.Equals(y.Id)
               && x.ContestYear == y.ContestYear
               && x.CityName == y.CityName
               && x.ContestFormat == y.ContestFormat
               && x.ContestStatus == y.ContestStatus
               && x.ChildBroadcasts.SequenceEqual(y.ChildBroadcasts)
               && x.Participants.SequenceEqual(y.Participants);
    }

    public int GetHashCode(Contest obj) => HashCode.Combine(obj.Id, obj.ContestYear, obj.CityName, (int)obj.ContestFormat,
        (int)obj.ContestStatus, obj.ChildBroadcasts, obj.Participants);
}
