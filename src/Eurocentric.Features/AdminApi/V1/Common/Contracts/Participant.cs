using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record Participant : IExampleProvider<Participant>
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

    public static Participant CreateExample() => new()
    {
        ParticipatingCountryId = ExampleIds.Country, ParticipantGroup = 2, ActName = "JJ", SongTitle = "Wasted Love"
    };
}
