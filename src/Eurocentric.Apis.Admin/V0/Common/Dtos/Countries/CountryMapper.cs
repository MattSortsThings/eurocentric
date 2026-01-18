using Riok.Mapperly.Abstractions;
using CountryAggregate = Eurocentric.Domain.Aggregates.Placeholders.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Common.Dtos.Countries.Country;

namespace Eurocentric.Apis.Admin.V0.Common.Dtos.Countries;

[Mapper]
internal static partial class CountryMapper
{
    internal static partial CountryDto MapToDto(CountryAggregate country);
}
