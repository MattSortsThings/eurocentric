namespace Eurocentric.Domain.Aggregates.V0;

/// <summary>
///     Represents a voting country that awards a set of televote points in a broadcast.
/// </summary>
public sealed class Televote
{
    /// <summary>
    ///     Gets the ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the televote has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; init; }
}
