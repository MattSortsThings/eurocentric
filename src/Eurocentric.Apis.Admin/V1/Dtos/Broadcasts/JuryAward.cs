using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a single points award given by a jury in a broadcast.
/// </summary>
public sealed record JuryAward : IDtoSchemaExampleProvider<JuryAward>
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     The numeric value of the points award.
    /// </summary>
    public int PointsValue { get; init; }

    public static JuryAward CreateExample() => new() { VotingCountryId = V1ExampleIds.CountryB, PointsValue = 12 };
}
