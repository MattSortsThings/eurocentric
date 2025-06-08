using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast.
/// </summary>
public sealed class Competitor : Entity
{
    private readonly List<JuryAward> _juryAwards = [];
    private readonly List<TelevoteAward> _televoteAwards = [];

    private Competitor()
    {
    }

    public Competitor(CountryId competingCountryId, int runningOrderPosition)
    {
        CompetingCountryId = competingCountryId;
        FinishingPosition = runningOrderPosition;
        RunningOrderPosition = runningOrderPosition;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate that the competitor represents.
    /// </summary>
    public CountryId CompetingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's finishing position in its broadcast.
    /// </summary>
    public int FinishingPosition { get; private set; } = 1;

    /// <summary>
    ///     Gets the competitor's running order position in its broadcast.
    /// </summary>
    public int RunningOrderPosition { get; private init; } = 1;

    /// <summary>
    ///     Gets a list of all the jury awards received by the competitor, ordered by descending points value then by ascending
    ///     voting country ID value.
    /// </summary>
    public IReadOnlyList<JuryAward> JuryAwards =>
        _juryAwards.OrderByDescending(award => award.PointsValue)
            .ThenBy(award => award.VotingCountryId.Value)
            .ToArray();

    /// <summary>
    ///     Gets a list of all the televote awards received by the competitor, ordered by descending points value then by
    ///     ascending voting country ID value.
    /// </summary>
    public IReadOnlyList<TelevoteAward> TelevoteAwards =>
        _televoteAwards.OrderByDescending(award => award.PointsValue)
            .ThenBy(award => award.VotingCountryId.Value)
            .ToArray();
}
