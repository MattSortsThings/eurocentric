using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

internal sealed class CountryEqualityComparer : IEqualityComparer<Country>
{
    public bool Equals(Country? x, Country? y)
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
               && x.CountryCode == y.CountryCode
               && x.CountryName == y.CountryName
               && x.ParticipatingContestIds.SequenceEqual(y.ParticipatingContestIds);
    }

    public int GetHashCode(Country obj) => HashCode.Combine(obj.Id,
        obj.CountryCode,
        obj.CountryName,
        obj.ParticipatingContestIds);
}
