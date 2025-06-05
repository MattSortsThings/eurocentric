using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

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
               && x.ParticipatingContests.SequenceEqual(y.ParticipatingContests);
    }

    public int GetHashCode(Country obj) => HashCode.Combine(obj.Id, obj.CountryCode, obj.CountryName, obj.ParticipatingContests);
}
