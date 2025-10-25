using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed record GlobalTelevote : ISchemaExampleProvider<GlobalTelevote>
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    public static GlobalTelevote CreateExample() =>
        new() { VotingCountryId = Guid.Parse("db0bb1f3-0709-4822-897e-5d1bd15c377a") };
}
