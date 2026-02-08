using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Analytics.Queryables;

public sealed record QueryableBroadcast
{
    /// <summary>
    ///     Gets or initializes the broadcast's date.
    /// </summary>
    public required DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     Gets or initializes the broadcast's contest stage.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets or initializes the broadcast's voting format.
    /// </summary>
    public required VotingFormat VotingFormat { get; init; }

    /// <summary>
    ///     Gets or initializes an ordered list of the broadcast's competing country codes.
    /// </summary>
    public required List<string> CompetingCountryCodes { get; init; }

    /// <summary>
    ///     Gets or initializes an ordered list of the broadcast's voting country codes.
    /// </summary>
    public required List<string> VotingCountryCodes { get; init; }
}
