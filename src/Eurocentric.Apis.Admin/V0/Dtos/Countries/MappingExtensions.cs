using Eurocentric.Apis.Admin.V0.Enums;
using ContestRoleDto = Eurocentric.Apis.Admin.V0.Dtos.Countries.ContestRole;
using ContestRoleValueObject = Eurocentric.Domain.V0.Aggregates.Countries.ContestRole;
using CountryAggregate = Eurocentric.Domain.V0.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Dtos.Countries.Country;

namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

internal static class MappingExtensions
{
    internal static CountryDto ToDto(this CountryAggregate aggregate)
    {
        return new CountryDto
        {
            Id = aggregate.Id,
            CountryCode = aggregate.CountryCode,
            CountryName = aggregate.CountryName,
            ContestRoles = aggregate.ContestRoles.Select(MapToDto).ToArray(),
        };
    }

    private static ContestRoleDto MapToDto(ContestRoleValueObject valueObject) =>
        new()
        {
            ContestId = valueObject.ContestId,
            ContestRoleType = valueObject.ContestRoleType.ToApiContestRoleType(),
        };
}
