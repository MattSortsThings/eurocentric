using Eurocentric.Apis.Public.V0.Contracts.Dtos;

namespace Eurocentric.Apis.Public.V0.Contracts.Queryables.Dtos;

internal static class OutboundMapping
{
    internal static QueryableBroadcast ToQueryableBroadcastDto(
        Domain.V0.Views.QueryableBroadcast broadcast
    )
    {
        return new QueryableBroadcast
        {
            BroadcastDate = broadcast.BroadcastDate,
            ContestYear = broadcast.ContestYear,
            ContestStage = broadcast.ContestStage.ToApiContestStage(),
            Competitors = broadcast.Competitors,
            Juries = broadcast.Juries,
            Televotes = broadcast.Televotes,
        };
    }

    internal static QueryableContest ToQueryableContestDto(Domain.V0.Views.QueryableContest contest)
    {
        return new QueryableContest
        {
            ContestYear = contest.ContestYear,
            CityName = contest.CityName,
            Participants = contest.Participants,
            UsesRestOfWorldTelevote = contest.UsesRestOfWorldTelevote,
        };
    }

    internal static QueryableCountry ToQueryableCountryDto(Domain.V0.Views.QueryableCountry country)
    {
        return new QueryableCountry
        {
            CountryCode = country.CountryCode,
            CountryName = country.CountryName,
        };
    }
}
