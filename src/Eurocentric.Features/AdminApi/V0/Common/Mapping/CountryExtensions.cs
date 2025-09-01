using Country = Eurocentric.Domain.V0.Entities.Country;
using CountryDto = Eurocentric.Features.AdminApi.V0.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V0.Common.Mapping;

internal static class CountryExtensions
{
    internal static CountryDto ToCountryDto(this Country country) => new()
    {
        Id = country.Id,
        CountryCode = country.CountryCode,
        CityName = country.CountryName,
        ParticipatingContestIds = country.ParticipatingContestIds.ToArray()
    };
}
