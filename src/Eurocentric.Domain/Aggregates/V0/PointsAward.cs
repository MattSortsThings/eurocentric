namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a points award given by a voter country in a broadcast.
/// </summary>
public abstract record PointsAward
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
