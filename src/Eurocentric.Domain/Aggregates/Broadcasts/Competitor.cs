using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a single competitor in a broadcast aggregate.
/// </summary>
public sealed class Competitor : Entity
{
    private readonly List<JuryAward> _juryAwards = [];
    private readonly List<TelevoteAward> _televoteAwards = [];

    private Competitor()
    {
    }

    public Competitor(CountryId competingCountryId, int runningOrderPosition = 1)
    {
        CompetingCountryId = competingCountryId;
        RunningOrderPosition = runningOrderPosition;
        FinishingPosition = runningOrderPosition;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate that the competitor represents.
    /// </summary>
    public CountryId CompetingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's finishing position in its broadcast.
    /// </summary>
    public int FinishingPosition { get; private set; }

    /// <summary>
    ///     Gets the competitor's running order position in its broadcast.
    /// </summary>
    public int RunningOrderPosition { get; private set; }

    /// <summary>
    ///     Gets a list of all the jury points awards received by the broadcast, ordered by descending points value then by
    ///     ascending voting country ID value.
    /// </summary>
    public IReadOnlyList<JuryAward> JuryAwards => _juryAwards.OrderByDescending(award => award.PointsValue)
        .ThenBy(award => award.VotingCountryId)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televote points awards received by the broadcast, ordered by descending points value then by
    ///     ascending voting country ID value.
    /// </summary>
    public IReadOnlyList<TelevoteAward> TelevoteAwards => _televoteAwards.OrderByDescending(award => award.PointsValue)
        .ThenBy(award => award.VotingCountryId)
        .ToArray()
        .AsReadOnly();
}
