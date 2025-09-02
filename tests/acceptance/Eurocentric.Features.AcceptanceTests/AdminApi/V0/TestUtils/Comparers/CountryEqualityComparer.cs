using Eurocentric.Features.AdminApi.V0.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.Comparers;

public sealed class CountryEqualityComparer : IEqualityComparer<Country>
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
               && x.ParticipatingContestIds.OrderBy(id => id).SequenceEqual(y.ParticipatingContestIds.OrderBy(id => id));
    }

    public int GetHashCode(Country obj) =>
        HashCode.Combine(obj.Id, obj.CountryCode, obj.CountryName, obj.ParticipatingContestIds);
}
