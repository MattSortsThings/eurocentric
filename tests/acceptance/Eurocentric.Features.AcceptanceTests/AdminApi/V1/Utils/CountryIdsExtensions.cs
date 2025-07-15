using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

internal static class CountryIdsExtensions
{
    internal static ContestParticipantSpecification[] ToContestParticipantSpecifications(this IEnumerable<Guid> countryIds) =>
        countryIds.Select(id => new ContestParticipantSpecification(id, "ActName", "SongTitle"))
            .ToArray();
}
