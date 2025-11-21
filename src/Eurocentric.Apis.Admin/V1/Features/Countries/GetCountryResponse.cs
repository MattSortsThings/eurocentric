using Eurocentric.Apis.Admin.V1.Dtos.Countries;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

public sealed record GetCountryResponse(Country Country)
{
    public bool Equals(GetCountryResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Country.Equals(other.Country);
    }

    public override int GetHashCode() => Country.GetHashCode();
}
