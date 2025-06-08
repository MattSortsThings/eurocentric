using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public static class GuidEnumerableExtensions
{
    public static ParticipantSpecification[] ToParticipantSpecifications(this IEnumerable<Guid> countryIds) =>
        countryIds.Select(id => new ParticipantSpecification { CountryId = id, ActName = "Act " + id, SongTitle = "Song " + id })
            .ToArray();
}
