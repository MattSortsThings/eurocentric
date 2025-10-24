using Eurocentric.Apis.Admin.V1.Enums;
using ContestRoleDto = Eurocentric.Apis.Admin.V1.Dtos.Countries.ContestRole;
using ContestRoleValueObject = Eurocentric.Domain.ValueObjects.ContestRole;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V1.Dtos.Countries.Country;

namespace Eurocentric.Apis.Admin.V1.Dtos.Countries;

internal static class MappingExtensions
{
    internal static CountryDto ToDto(this CountryAggregate aggregate)
    {
        return new CountryDto
        {
            Id = aggregate.Id.Value,
            CountryCode = aggregate.CountryCode.Value,
            CountryName = aggregate.CountryName.Value,
            ContestRoles = aggregate.ContestRoles.Select(MapToDto).ToArray(),
        };
    }

    internal static ContestRoleDto MapToDto(ContestRoleValueObject valueObject)
    {
        return new ContestRoleDto
        {
            ContestId = valueObject.ContestId.Value,
            ContestRoleType = valueObject.ContestRoleType.ToApiContestRoleType(),
        };
    }
}
