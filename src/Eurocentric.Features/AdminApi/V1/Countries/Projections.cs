using System.Linq.Expressions;
using DomainCountry = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Contracts.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

internal static class Projections
{
    internal static readonly Expression<Func<DomainCountry, CountryDto>> CountryToCountryDto = country => new CountryDto
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        CountryName = country.CountryName.Value,
        ParticipatingContestIds = country.ParticipatingContestIds.Select(id => id.Value).ToArray()
    };
}
