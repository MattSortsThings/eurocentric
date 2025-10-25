using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

public sealed record CreateCountryResponse(Country Country) : ISchemaExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample() => new(Country.CreateExample() with { ContestRoles = [] });
}
