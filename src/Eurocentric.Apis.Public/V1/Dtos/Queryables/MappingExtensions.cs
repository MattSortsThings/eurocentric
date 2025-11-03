using QueryableContestDto = Eurocentric.Apis.Public.V1.Dtos.Queryables.QueryableContest;
using QueryableContestRow = Eurocentric.Domain.Analytics.Queryables.QueryableContest;
using QueryableCountryDto = Eurocentric.Apis.Public.V1.Dtos.Queryables.QueryableCountry;
using QueryableCountryRow = Eurocentric.Domain.Analytics.Queryables.QueryableCountry;

namespace Eurocentric.Apis.Public.V1.Dtos.Queryables;

internal static class MappingExtensions
{
    internal static QueryableContestDto ToDto(this QueryableContestRow row)
    {
        return new QueryableContestDto
        {
            ContestYear = row.ContestYear,
            CityName = row.CityName,
            Participants = row.Participants,
            UsesRestOfWorldTelevote = row.UsesRestOfWorldTelevote,
        };
    }

    internal static QueryableCountryDto ToDto(this QueryableCountryRow row) =>
        new() { CountryCode = row.CountryCode, CountryName = row.CountryName };
}
