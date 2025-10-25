using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed record Participant : ISchemaExampleProvider<Participant>
{
    /// <summary>
    ///     The ID of the participating country.
    /// </summary>
    public Guid ParticipatingCountryId { get; init; }

    /// <summary>
    ///     The participant's semi-final draw.
    /// </summary>
    public SemiFinalDraw SemiFinalDraw { get; init; }

    /// <summary>
    ///     The participant's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     The participant's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;

    public static Participant CreateExample() =>
        new()
        {
            ParticipatingCountryId = Guid.Parse("a54ef079-5ef6-4867-8a48-38ab8068ed1c"),
            SemiFinalDraw = SemiFinalDraw.SemiFinal2,
            ActName = "JJ",
            SongTitle = "Wasted Love",
        };
}
