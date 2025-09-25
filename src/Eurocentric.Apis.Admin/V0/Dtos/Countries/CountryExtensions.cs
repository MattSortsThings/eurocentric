using Eurocentric.Apis.Admin.V0.Enums;
using CountryAggregate = Eurocentric.Domain.V0.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Dtos.Countries.Country;

namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

internal static class CountryExtensions
{
    internal static CountryDto ToDto(this CountryAggregate country)
    {
        return new CountryDto
        {
            Id = country.Id,
            CountryCode = country.CountryCode,
            CountryName = country.CountryName,
            ContestRoles = country
                .ContestRoles.Select(role => new ContestRole
                {
                    ContestId = role.ContestId,
                    ContestRoleType = role.ContestRoleType.ToApiContestRoleType(),
                })
                .ToArray(),
        };
    }
}
