using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

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
               && x.Year == y.Year
               && x.CityName == y.CityName
               && x.Format == y.Format
               && x.Status == y.Status
               && x.BroadcastMemos.SequenceEqual(y.BroadcastMemos)
               && x.Participants.SequenceEqual(y.Participants);
    }

    public int GetHashCode(Contest obj) => HashCode.Combine(obj.Id,
        obj.Year,
        obj.CityName,
        (int)obj.Format,
        (int)obj.Status,
        obj.BroadcastMemos,
        obj.Participants);
}
