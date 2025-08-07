using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a competitor in a broadcast.
/// </summary>
public sealed class Competitor : Entity
{
    private readonly List<JuryAward> _juryAwards = [];
    private readonly List<TelevoteAward> _televoteAwards = [];

    [UsedImplicitly(Reason = "EF Core")]
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
    ///     Gets the ID of the country aggregate the competitor represents.
    /// </summary>
    public CountryId CompetingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's running order position in its broadcast.
    /// </summary>
    public int RunningOrderPosition { get; private init; }

    /// <summary>
    ///     Gets the competitor's finishing position in its broadcast.
    /// </summary>
    public int FinishingPosition { get; private init; }

    /// <summary>
    ///     Gets a list of all the jury awards the competitor has received.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<JuryAward> JuryAwards => _juryAwards.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televote awards the competitor has received.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<TelevoteAward> TelevoteAwards => _televoteAwards.ToArray().AsReadOnly();
}
