using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

public sealed record CreateCountryResponse(Country Country) : IDtoSchemaExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample() => new(Country.CreateExample() with { ContestRoles = [] });

    public bool Equals(CreateCountryResponse? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Country.Equals(other.Country);
    }

    public override int GetHashCode() => Country.GetHashCode();
}
