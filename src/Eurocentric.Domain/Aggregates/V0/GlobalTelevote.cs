namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a voting country acting as a global televote in a contest.
/// </summary>
public sealed class GlobalTelevote
{
    /// <summary>
    ///     Gets the ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }
}
