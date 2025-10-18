namespace Eurocentric.Domain.V0.Aggregates.Broadcasts;

/// <summary>
///     Represents a voter in a broadcast.
/// </summary>
public abstract record Voter
{
    /// <summary>
    ///     Gets the voting country's ID.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets a value indicating whether the voting country has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; init; }
}
