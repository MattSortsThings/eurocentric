using Eurocentric.Apis.Public.V0.Enums;
using QueryableBroadcastDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableBroadcast;
using QueryableBroadcastRecord = Eurocentric.Domain.V0.Queries.Queryables.QueryableBroadcast;
using QueryableContestDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableContest;
using QueryableContestRecord = Eurocentric.Domain.V0.Queries.Queryables.QueryableContest;
using QueryableCountryDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableCountry;
using QueryableCountryRecord = Eurocentric.Domain.V0.Queries.Queryables.QueryableCountry;

namespace Eurocentric.Apis.Public.V0.Dtos.Queryables;

internal static class MappingExtensions
{
    internal static QueryableBroadcastDto ToDto(this QueryableBroadcastRecord record)
    {
        return new QueryableBroadcastDto
        {
            BroadcastDate = record.BroadcastDate,
            ContestYear = record.ContestYear,
            CityName = record.CityName,
            ContestStage = record.ContestStage.ToApiContestStage(),
            Competitors = record.Competitors,
            Juries = record.Juries,
            Televotes = record.Televotes,
        };
    }

    internal static QueryableContestDto ToDto(this QueryableContestRecord record)
    {
        return new QueryableContestDto
        {
            ContestYear = record.ContestYear,
            CityName = record.CityName,
            Participants = record.Participants,
            UsesRestOfWorldTelevote = record.UsesRestOfWorldTelevote,
        };
    }

    internal static QueryableCountryDto ToDto(this QueryableCountryRecord record) =>
        new() { CountryCode = record.CountryCode, CountryName = record.CountryName };
}
