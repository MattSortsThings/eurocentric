using Eurocentric.Apis.Public.V0.Common.Enums;

namespace Eurocentric.Apis.Public.V0.Common.Models.Queryables;

public sealed record QueryableBroadcast
{
    /// <summary>
    ///     The broadcast's date.
    /// </summary>
    public required DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The broadcast's contest stage.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The broadcast's voting format.
    /// </summary>
    public required VotingFormat VotingFormat { get; init; }

    /// <summary>
    ///     An ordered array of the broadcast's competing country codes.
    /// </summary>
    public required string[] CompetingCountryCodes { get; init; }

    /// <summary>
    ///     An ordered array of the broadcast's voting country codes.
    /// </summary>
    public required string[] VotingCountryCodes { get; init; }
}
