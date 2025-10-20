using Eurocentric.Apis.Admin.V0.Dtos.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries.TestUtils;

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
            && x.ContestRoles.OrderBy(role => role.ContestId)
                .SequenceEqual(y.ContestRoles.OrderBy(role => role.ContestId));
    }

    public int GetHashCode(Country obj) => HashCode.Combine(obj.Id, obj.CountryCode, obj.CountryName, obj.ContestRoles);
}
