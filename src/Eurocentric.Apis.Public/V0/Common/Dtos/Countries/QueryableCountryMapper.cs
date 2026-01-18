using Eurocentric.Domain.Queries.Placeholders;
using Riok.Mapperly.Abstractions;

namespace Eurocentric.Apis.Public.V0.Common.Dtos.Countries;

[Mapper]
internal static partial class QueryableCountryMapper
{
    internal static partial Country ToCountryDto(QueryableCountry queryableCountry);
}
