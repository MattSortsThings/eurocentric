using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

/// <summary>
///     Represents a participant in a contest.
/// </summary>
public sealed record Participant : IExampleProvider<Participant>
{
    /// <summary>
    ///     The ID of the participating country.
    /// </summary>
    public Guid ParticipatingCountryId { get; init; }

    /// <summary>
    ///     The Semi-Final drawn by the participant.
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

    public static Participant CreateExample() => new()
    {
        ParticipatingCountryId = ExampleValues.CountryId1Of2,
        SemiFinalDraw = SemiFinalDraw.SemiFinal2,
        ActName = ExampleValues.ActName,
        SongTitle = ExampleValues.SongTitle
    };
}
