using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast
/// </summary>
public sealed record Competitor : IDtoSchemaExampleProvider<Competitor>
{
    /// <summary>
    ///     The ID of the competing country.
    /// </summary>
    public Guid CompetingCountryId { get; init; }

    /// <summary>
    ///     The competitor's running order spot in the broadcast.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     The competitor's finishing position in the broadcast.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     An array of all the jury awards received by the competitor.
    /// </summary>
    public JuryAward[] JuryAwards { get; init; } = [];

    /// <summary>
    ///     An array of all the televote awards received by the competitor.
    /// </summary>
    public TelevoteAward[] TelevoteAwards { get; init; } = [];

    public static Competitor CreateExample() =>
        new()
        {
            RunningOrderSpot = 1,
            CompetingCountryId = V1ExampleIds.CountryA,
            FinishingPosition = 1,
            JuryAwards = [JuryAward.CreateExample()],
            TelevoteAwards = [TelevoteAward.CreateExample()],
        };
}
