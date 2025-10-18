namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed record GlobalTelevote
{
    /// <summary>
    ///     Gets the voting country's ID.
    /// </summary>
    public Guid VotingCountryId { get; init; }
}
