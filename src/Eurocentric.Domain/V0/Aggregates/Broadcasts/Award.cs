using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Broadcasts;

/// <summary>
///     Represents a points award in a broadcast.
/// </summary>
public abstract record Award
{
    /// <summary>
    ///     Gets the voting country's ID.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets the numeric value of the points award.
    /// </summary>
    public PointsValue PointsValue { get; init; }
}
