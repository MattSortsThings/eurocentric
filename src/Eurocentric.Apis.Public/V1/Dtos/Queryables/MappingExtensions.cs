using QueryableCountryDto = Eurocentric.Apis.Public.V1.Dtos.Queryables.QueryableCountry;
using QueryableCountryRow = Eurocentric.Domain.Analytics.Queryables.QueryableCountry;

namespace Eurocentric.Apis.Public.V1.Dtos.Queryables;

internal static class MappingExtensions
{
    internal static QueryableCountryDto ToDto(this QueryableCountryRow row) =>
        new() { CountryCode = row.CountryCode, CountryName = row.CountryName };
}
