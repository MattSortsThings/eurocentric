using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Queries.Placeholders;

/// <summary>
///     Represents a queryable contest.
/// </summary>
public sealed record QueryableContest
{
    /// <summary>
    ///     The contest's year.
    /// </summary>
    public required int ContestYear { get; init; }

    /// <summary>
    ///     The contest's host city name.
    /// </summary>
    public required string CityName { get; init; }

    /// <summary>
    ///     The number of participants in the contest.
    /// </summary>
    public required int Participants { get; init; }

    /// <summary>
    ///     The contest's Semi-Final voting rules.
    /// </summary>
    public required VotingRules SemiFinalVotingRules { get; init; }

    /// <summary>
    ///     The contest's Grand Final voting rules.
    /// </summary>
    public required VotingRules GrandFinalVotingRules { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the contest uses a global televote.
    /// </summary>
    public required bool UsesGlobalTelevote { get; init; }
}
