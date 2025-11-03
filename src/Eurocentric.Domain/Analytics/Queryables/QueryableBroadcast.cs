using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Queryables;

/// <summary>
///     A queryable broadcast.
/// </summary>
/// <remarks>A broadcast is queryable if its parent contest is queryable.</remarks>
public sealed record QueryableBroadcast
{
    /// <summary>
    ///     Gets the date on which the broadcast is televised.
    /// </summary>
    public DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     Gets the UK English name of the city in which the contest is held.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     Gets the number of competitors in the broadcast.
    /// </summary>
    public int Competitors { get; init; }

    /// <summary>
    ///     Gets the number of juries in the broadcast.
    /// </summary>
    public int Juries { get; init; }

    /// <summary>
    ///     Gets the number of televotes in the broadcast.
    /// </summary>
    public int Televotes { get; init; }
}
