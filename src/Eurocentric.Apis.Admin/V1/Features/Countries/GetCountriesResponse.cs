using Eurocentric.Apis.Admin.V1.Dtos.Countries;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

public sealed record GetCountriesResponse(Country[] Countries)
{
    public bool Equals(GetCountriesResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Countries.SequenceEqual(other.Countries);
    }

    public override int GetHashCode() => Countries.GetHashCode();
}
