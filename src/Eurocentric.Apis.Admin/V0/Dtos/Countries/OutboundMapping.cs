using Eurocentric.Apis.Admin.V0.Enums;
using CountryAggregate = Eurocentric.Domain.V0.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Dtos.Countries.Country;

namespace Eurocentric.Apis.Admin.V0.Dtos.Countries;

internal static class OutboundMapping
{
    internal static CountryDto ToDto(this CountryAggregate aggregate)
    {
        return new CountryDto
        {
            Id = aggregate.Id,
            CountryCode = aggregate.CountryCode,
            CountryName = aggregate.CountryName,
            ContestRoles = aggregate
                .ContestRoles.Select(role => new ContestRole
                {
                    ContestId = role.ContestId,
                    ContestRoleType = role.ContestRoleType.ToApiContestRoleType(),
                })
                .ToArray(),
        };
    }
}
