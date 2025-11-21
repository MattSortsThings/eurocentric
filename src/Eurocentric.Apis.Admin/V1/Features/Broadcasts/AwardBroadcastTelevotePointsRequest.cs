using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

public sealed record AwardBroadcastTelevotePointsRequest
    : IDtoSchemaExampleProvider<AwardBroadcastTelevotePointsRequest>
{
    /// <summary>
    ///     The voting country ID of the televote to award points.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     The competing country IDs in rank order from first to last.
    /// </summary>
    public required Guid[] RankedCompetingCountryIds { get; init; }

    public static AwardBroadcastTelevotePointsRequest CreateExample() =>
        new() { VotingCountryId = V1ExampleIds.CountryB, RankedCompetingCountryIds = [V1ExampleIds.CountryA] };
}
