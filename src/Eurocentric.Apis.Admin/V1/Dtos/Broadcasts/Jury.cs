using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a jury in a broadcast.
/// </summary>
public sealed record Jury : IDtoSchemaExampleProvider<Jury>
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the jury has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; init; }

    public static Jury CreateExample() => new() { VotingCountryId = V1ExampleIds.CountryA };
}
