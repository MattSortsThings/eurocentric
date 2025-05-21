using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a country competing in a broadcast.
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
        RunningOrderPosition = runningOrderPosition;
        FinishingPosition = runningOrderPosition;
    }

    /// <summary>
    ///     Gets the ID of the competing country.
    /// </summary>
    public CountryId CompetingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's running order position in its broadcast.
    /// </summary>
    public int RunningOrderPosition { get; private init; } = 1;

    /// <summary>
    ///     Gets the competitor's finishing position in its broadcast.
    /// </summary>
    public int FinishingPosition { get; internal set; } = 1;

    /// <summary>
    ///     Gets a list of all the jury awards the competitor has received, ordered by descending points value then by
    ///     ascending voting country ID value.
    /// </summary>
    public IReadOnlyList<JuryAward> JuryAwards => _juryAwards
        .OrderByDescending(award => award.PointsValue)
        .ThenBy(award => award.VotingCountryId)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televote awards the competitor has received, ordered by descending points value then by
    ///     ascending voting country ID value.
    /// </summary>
    public IReadOnlyList<TelevoteAward> TelevoteAwards => _televoteAwards
        .OrderByDescending(award => award.PointsValue)
        .ThenBy(award => award.VotingCountryId)
        .ToArray()
        .AsReadOnly();

    internal void ReceiveAward(JuryAward juryAward) => _juryAwards.Add(juryAward);

    internal void ReceiveAward(TelevoteAward award) => _televoteAwards.Add(award);
}
