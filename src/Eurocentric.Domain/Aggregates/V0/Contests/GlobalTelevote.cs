namespace Eurocentric.Domain.Aggregates.V0.Contests;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed class GlobalTelevote
{
    /// <summary>
    ///     Gets or initializes the global televote's voting country ID.
    /// </summary>
    public required Guid VotingCountryId { get; init; }
}
