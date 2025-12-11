namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a voter in a broadcast.
/// </summary>
public abstract record Voter
{
    /// <summary>
    ///     Gets the ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the voter has awarded their points.
    /// </summary>
    public bool PointsAwarded { get; init; }
}
