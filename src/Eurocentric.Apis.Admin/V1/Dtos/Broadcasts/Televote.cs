namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a televote in a broadcast.
/// </summary>
public sealed record Televote
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the televote has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; init; }
}
