namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record Competitor
{
    /// <summary>
    ///     The competing country ID.
    /// </summary>
    public required Guid CompetingCountryId { get; init; }

    /// <summary>
    ///     The competitor's finishing position in its broadcast.
    /// </summary>
    public required int FinishingPosition { get; init; }

    /// <summary>
    ///     The competitor's running order position in its broadcast.
    /// </summary>
    public required int RunningOrderPosition { get; init; }

    /// <summary>
    ///     The jury awards received by the competitor.
    /// </summary>
    public required PointsAward[] JuryAwards { get; init; }

    /// <summary>
    ///     The televote awards received by the competitor.
    /// </summary>
    public required PointsAward[] TelevoteAwards { get; init; }
}
