using Eurocentric.Apis.Public.V0.Enums;
using QueryableBroadcastDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableBroadcast;
using QueryableBroadcastView = Eurocentric.Domain.V0.Views.QueryableBroadcast;
using QueryableContestDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableContest;
using QueryableContestView = Eurocentric.Domain.V0.Views.QueryableContest;
using QueryableCountryDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableCountry;
using QueryableCountryView = Eurocentric.Domain.V0.Views.QueryableCountry;

namespace Eurocentric.Apis.Public.V0.Dtos.Queryables;

internal static class OutboundMapping
{
    internal static QueryableBroadcastDto ToDto(this QueryableBroadcastView view)
    {
        return new QueryableBroadcastDto
        {
            BroadcastDate = view.BroadcastDate,
            ContestYear = view.ContestYear,
            ContestStage = view.ContestStage.ToApiContestStage(),
            Competitors = view.Competitors,
            Juries = view.Juries,
            Televotes = view.Televotes,
        };
    }

    internal static QueryableContestDto ToDto(this QueryableContestView view)
    {
        return new QueryableContestDto
        {
            ContestYear = view.ContestYear,
            CityName = view.CityName,
            Participants = view.Participants,
            UsesRestOfWorldTelevote = view.UsesRestOfWorldTelevote,
        };
    }

    internal static QueryableCountryDto ToDto(this QueryableCountryView view)
    {
        return new QueryableCountryDto
        {
            CountryCode = view.CountryCode,
            CountryName = view.CountryName,
        };
    }
}
