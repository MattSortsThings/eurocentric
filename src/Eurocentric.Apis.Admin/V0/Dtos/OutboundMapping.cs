using System.Linq.Expressions;
using Eurocentric.Apis.Admin.V0.Enums;
using CountryEntity = Eurocentric.Domain.V0Entities.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Dtos.Country;

namespace Eurocentric.Apis.Admin.V0.Dtos;

internal static class OutboundMapping
{
    internal static readonly Expression<Func<CountryEntity, CountryDto>> CountryToCountryDto = country => new CountryDto
    {
        Id = country.Id,
        CountryCode = country.CountryCode,
        CountryName = country.CountryName,
        ContestRoles = country.ContestRoles.Select(role => new ContestRole
        {
            ContestId = role.ContestId, ContestRoleType = (ContestRoleType)(int)role.ContestRoleType
        }).ToArray()
    };
}
