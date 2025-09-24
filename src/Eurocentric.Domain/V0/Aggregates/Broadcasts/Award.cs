namespace Eurocentric.Domain.V0.Aggregates.Broadcasts;

/// <summary>
///     Represents a points award in a broadcast.
/// </summary>
public abstract record Award
{
    /// <summary>
    ///     Gets the ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets the numeric points value of the award.
    /// </summary>
    public int PointsValue { get; init; }
}
