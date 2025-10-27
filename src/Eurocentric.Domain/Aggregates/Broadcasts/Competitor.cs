using Eurocentric.Domain.Core;
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
    private Competitor() { }

    internal Competitor(
        CountryId competingCountryId,
        RunningOrderSpot runningOrderSpot,
        FinishingPosition finishingPosition
    )
    {
        CompetingCountryId = competingCountryId;
        RunningOrderSpot = runningOrderSpot;
        FinishingPosition = finishingPosition;
    }

    /// <summary>
    ///     Gets the ID of the competing country.
    /// </summary>
    public CountryId CompetingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's running order spot in the broadcast.
    /// </summary>
    public RunningOrderSpot RunningOrderSpot { get; private init; } = null!;

    /// <summary>
    ///     Gets the competitor's finishing position in the broadcast.
    /// </summary>
    public FinishingPosition FinishingPosition { get; internal set; } = null!;

    /// <summary>
    ///     Gets a list of all the jury awards the competitor received.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the country's jury award list.</remarks>
    public IReadOnlyList<JuryAward> JuryAwards => _juryAwards.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televote awards the competitor received.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the country's televote award list.</remarks>
    public IReadOnlyList<TelevoteAward> TelevoteAwards => _televoteAwards.ToArray().AsReadOnly();
}
