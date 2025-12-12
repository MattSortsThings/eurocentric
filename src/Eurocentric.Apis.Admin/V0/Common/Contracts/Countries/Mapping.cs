using Eurocentric.Apis.Admin.V0.Common.Enums;
using ContestRoleDto = Eurocentric.Apis.Admin.V0.Common.Contracts.Countries.ContestRole;
using ContestRoleValueObject = Eurocentric.Domain.Aggregates.V0.ContestRole;
using CountryAggregate = Eurocentric.Domain.Aggregates.V0.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Common.Contracts.Countries.Country;

namespace Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;

internal static class Mapping
{
    internal static GetCountriesResponse ToGetCountriesResponse(this IEnumerable<CountryAggregate> countries) =>
        new(countries.Select(country => country.ToDto()).ToArray());

    private static ContestRoleDto MapToDto(ContestRoleValueObject contestRole)
    {
        return new ContestRoleDto
        {
            ContestId = contestRole.ContestId,
            ContestRoleType = Enum.Parse<ContestRoleType>(contestRole.ContestRoleType.ToString()),
        };
    }

    extension(CountryAggregate country)
    {
        internal GetCountryResponse ToGetCountryResponse() => new(country.ToDto());

        internal CreateCountryResponse ToCreateCountryResponse() => new(country.ToDto());

        private CountryDto ToDto()
        {
            return new CountryDto
            {
                Id = country.Id,
                CountryCode = country.CountryCode,
                CountryName = country.CountryName,
                ContestRoles = country.ContestRoles.Select(MapToDto).ToArray(),
            };
        }
    }
}
