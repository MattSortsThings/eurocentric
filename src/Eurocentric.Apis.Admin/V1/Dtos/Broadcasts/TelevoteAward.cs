namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a single points award given by a televote in a broadcast.
/// </summary>
public sealed record TelevoteAward
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }

    /// <summary>
    ///     The numeric value of the points award.
    /// </summary>
    public int PointsValue { get; init; }
}
