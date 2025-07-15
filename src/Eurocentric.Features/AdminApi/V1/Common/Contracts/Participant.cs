namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record Participant
{
    /// <summary>
    ///     The ID of the participating country.
    /// </summary>
    public required Guid ParticipatingCountryId { get; init; }

    /// <summary>
    ///     The participant's group in its contest (0, 1 or 2).
    /// </summary>
    public required int ParticipantGroup { get; init; }

    /// <summary>
    ///     The participant's act name.
    /// </summary>
    public string? ActName { get; init; }

    /// <summary>
    ///     The participant's song title.
    /// </summary>
    public string? SongTitle { get; init; }
}
