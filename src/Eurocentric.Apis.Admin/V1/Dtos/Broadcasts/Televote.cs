using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a televote in a broadcast.
/// </summary>
public sealed record Televote : ISchemaExampleProvider<Televote>
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the televote has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; init; }

    public static Televote CreateExample() =>
        new() { VotingCountryId = V1ExampleValues.CountryId2Of2, PointsAwarded = true };
}
