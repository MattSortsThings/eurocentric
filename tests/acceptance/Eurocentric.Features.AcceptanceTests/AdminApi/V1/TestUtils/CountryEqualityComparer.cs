using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

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
               && x.Name == y.Name
               && x.ContestMemos.SequenceEqual(y.ContestMemos);
    }

    public int GetHashCode(Country obj) => HashCode.Combine(obj.Id, obj.CountryCode, obj.Name, obj.ContestMemos);
}
