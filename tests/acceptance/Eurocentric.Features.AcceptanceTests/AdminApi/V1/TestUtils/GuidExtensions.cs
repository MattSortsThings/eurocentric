using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

internal static class GuidExtensions
{
    internal static ContestParticipantDatum ToContestParticipantDatum(this Guid countryId) => new()
    {
        CountryId = countryId, ActName = "ActName " + countryId, SongTitle = "SongTitle " + countryId
    };

    internal static ContestParticipantDatum[] ToContestParticipantData(this IEnumerable<Guid> countryIds) =>
        countryIds.Select(id => id.ToContestParticipantDatum()).ToArray();
}
