using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public sealed record CreateCountryResponse(Country Country) : IDtoSchemaExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample() => new(Country.CreateExample() with { ContestRoles = [] });
}
