using ContestRole = Eurocentric.Domain.V0.Aggregates.Countries.ContestRole;
using ContestRoleDto = Eurocentric.Apis.Admin.V0.Contracts.Dtos.ContestRole;
using ContestRoleType = Eurocentric.Domain.Enums.ContestRoleType;
using ContestRoleTypeDto = Eurocentric.Apis.Admin.V0.Contracts.Dtos.ContestRoleType;
using Country = Eurocentric.Domain.V0.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Contracts.Dtos.Country;

namespace Eurocentric.Apis.Admin.V0.Contracts.Mapping;

internal static class OutboundMapping
{
    internal static CountryDto ToCountryDto(this Country country)
    {
        return new CountryDto
        {
            Id = country.Id,
            CountryCode = country.CountryCode,
            CountryName = country.CountryName,
            ContestRoles = country.ContestRoles.Select(role => role.ToContestRoleDto()).ToArray(),
        };
    }

    private static ContestRoleDto ToContestRoleDto(this ContestRole role)
    {
        return new ContestRoleDto
        {
            ContestId = role.ContestId,
            ContestRoleType = role.ContestRoleType.ToContestRoleTypeDto(),
        };
    }

    private static ContestRoleTypeDto ToContestRoleTypeDto(this ContestRoleType roleType)
    {
        return roleType switch
        {
            ContestRoleType.Participant => ContestRoleTypeDto.Participant,
            ContestRoleType.GlobalTelevote => ContestRoleTypeDto.GlobalTelevote,
            _ => throw new ArgumentException($"Value {roleType} not supported."),
        };
    }
}
