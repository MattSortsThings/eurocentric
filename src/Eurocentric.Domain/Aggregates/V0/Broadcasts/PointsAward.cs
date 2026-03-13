using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.V0.Broadcasts;

/// <summary>
///     Represents a points award from a voting country in a broadcast.
/// </summary>
public sealed class PointsAward
{
    /// <summary>
    ///     Gets or initializes the points award's voting country ID.
    /// </summary>
    public required Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets or initializes the points award's voting method.
    /// </summary>
    public required VotingMethod VotingMethod { get; init; }

    /// <summary>
    ///     Gets or initializes the points award's points value.
    /// </summary>
    public required int PointsValue { get; init; }
}
