using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateContestBroadcastRequest
{
    /// <summary>
    ///     The date on which the broadcast is televised.
    /// </summary>
    public required DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The IDs of the competing countries, in broadcast running order, with vacant running order spots represented by
    ///     <see langword="null" /> values.
    /// </summary>
    public required Guid?[] CompetingCountryIds { get; init; }
}
