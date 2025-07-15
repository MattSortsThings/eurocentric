using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

internal static class DummyBroadcastGenerator
{
    internal static Broadcast Create()
    {
        BroadcastId broadcastId = BroadcastId.FromValue(ExampleIds.Broadcast);
        ContestId parentContestId = ContestId.FromValue(ExampleIds.Contest);

        List<CountryId> countryIds =
        [
            CountryId.FromValue(ExampleIds.CountryAt),
            CountryId.FromValue(ExampleIds.CountryIt),
            CountryId.FromValue(ExampleIds.CountryXx)
        ];

        DateOnly broadcastDate = DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd");

        List<Competitor> competitors = countryIds.Select((countryId, index) =>
                new Competitor(countryId, index + 1))
            .ToList();

        List<Jury> juries = countryIds.Select(countryId => new Jury(countryId))
            .ToList();

        List<Televote> televotes = countryIds.Select(countryId => new Televote(countryId))
            .ToList();

        return new Broadcast(broadcastId,
            broadcastDate,
            parentContestId,
            ContestStage.GrandFinal,
            competitors,
            juries,
            televotes);
    }
}
