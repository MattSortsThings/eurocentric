using Country = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V1.Common.Mapping;

internal static class CountryExtensions
{
    internal static CountryDto ToCountryDto(this Country country) => new()
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        CountryName = country.CountryName.Value,
        ParticipatingContestIds = country.ParticipatingContestIds.Select(contestId => contestId.Value).ToArray()
    };
}
