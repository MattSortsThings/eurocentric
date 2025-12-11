namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed record GlobalTelevote
{
    /// <summary>
    ///     Gets the ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }
}
