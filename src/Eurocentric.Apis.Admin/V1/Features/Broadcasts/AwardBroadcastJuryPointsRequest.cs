using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Broadcasts;

public sealed record AwardBroadcastJuryPointsRequest : ISchemaExampleProvider<AwardBroadcastJuryPointsRequest>
{
    /// <summary>
    ///     The voting country ID of the jury to award points.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     The competing country IDs in rank order from first to last.
    /// </summary>
    public required Guid[] RankedCompetingCountryIds { get; init; }

    public static AwardBroadcastJuryPointsRequest CreateExample() =>
        new()
        {
            VotingCountryId = V1ExampleValues.CountryId1Of2,
            RankedCompetingCountryIds = [V1ExampleValues.CountryId2Of2],
        };
}
