using Eurocentric.Apis.Public.V1.Dtos.Queryables;

namespace Eurocentric.Apis.Public.V1.Features.Queryables;

public sealed record GetQueryableCountriesResponse(QueryableCountry[] QueryableCountries)
{
    public bool Equals(GetQueryableCountriesResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || QueryableCountries.SequenceEqual(other.QueryableCountries);
    }

    public override int GetHashCode() => QueryableCountries.GetHashCode();
}
