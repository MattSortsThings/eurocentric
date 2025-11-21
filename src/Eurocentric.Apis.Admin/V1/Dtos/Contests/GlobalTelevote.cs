namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed record GlobalTelevote
{
    /// <summary>
    ///     The ID of the voting country.
    /// </summary>
    public Guid VotingCountryId { get; init; }
}
