using Eurocentric.Apis.Admin.V1.Dtos.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

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
            && x.CountryCode.Equals(y.CountryCode, StringComparison.Ordinal)
            && x.CountryName.Equals(y.CountryName, StringComparison.Ordinal)
            && x.ContestRoles.OrderBy(role => role.ContestId)
                .SequenceEqual(y.ContestRoles.OrderBy(role => role.ContestId));
    }

    public int GetHashCode(Country obj) => HashCode.Combine(obj.Id, obj.CountryCode, obj.CountryName, obj.ContestRoles);
}
