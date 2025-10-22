using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public sealed record CreateCountryResponse(Country Country) : ISchemaExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample()
    {
        Country country = Country.CreateExample() with { ContestRoles = [] };

        return new CreateCountryResponse(country);
    }
}
