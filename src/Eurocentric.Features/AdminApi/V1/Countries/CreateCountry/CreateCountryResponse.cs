using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;

public sealed record CreateCountryResponse(Country Country) : IExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample() => new(Country.CreateExample() with { ParticipatingContestIds = [] });
}
