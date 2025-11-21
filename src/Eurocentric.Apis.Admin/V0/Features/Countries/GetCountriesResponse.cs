using Eurocentric.Apis.Admin.V0.Dtos.Countries;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

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
