using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a broadcast.
/// </summary>
public sealed class Broadcast : AggregateRoot<BroadcastId>
{
    private readonly List<Competitor> _competitors;
    private readonly List<Jury> _juries;
    private readonly List<Televote> _televotes;

    [UsedImplicitly(Reason = "EF Core")]
    private Broadcast()
    {
        _competitors = [];
        _juries = [];
        _televotes = [];
    }

    internal Broadcast(
        BroadcastId id,
        BroadcastDate broadcastDate,
        ContestStage contestStage,
        ContestId parentContestId,
        List<Competitor> competitors,
        List<Jury> juries,
        List<Televote> televotes
    )
        : base(id)
    {
        _competitors = competitors;
        _juries = juries;
        _televotes = televotes;
        BroadcastDate = broadcastDate;
        ParentContestId = parentContestId;
        ContestStage = contestStage;
    }

    /// <summary>
    ///     Gets the date on which the broadcast is televised.
    /// </summary>
    public BroadcastDate BroadcastDate { get; private init; } = null!;

    /// <summary>
    ///     Gets the ID of the broadcast's parent contest.
    /// </summary>
    public ContestId ParentContestId { get; private init; } = null!;

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; private init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the broadcast is completed.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    ///     Gets a list of all the broadcast's competitors.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the broadcast's competitor list.</remarks>
    public IReadOnlyList<Competitor> Competitors => _competitors.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the broadcast's juries.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the broadcast's jury list.</remarks>
    public IReadOnlyList<Jury> Juries => _juries.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the broadcast's televotes.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the broadcast's televote list.</remarks>
    public IReadOnlyList<Televote> Televotes => _televotes.ToArray().AsReadOnly();

    public static Broadcast CreateDummyBroadcast(BroadcastId id, BroadcastDate date, ContestStage contestStage)
    {
        ContestId parentContestId = ContestId.FromValue(Guid.NewGuid());

        CountryId[] countryIds = Enumerable
            .Range(0, 3)
            .Select(_ => Guid.NewGuid())
            .Select(CountryId.FromValue)
            .ToArray();

        var competitorNumbers = RunningOrderSpot
            .CreateSequence(3)
            .Zip(
                FinishingPosition.CreateSequence(3),
                (runningOrderSpot, finishingPosition) => new { runningOrderSpot, finishingPosition }
            );

        List<Competitor> competitors = countryIds
            .Zip(
                competitorNumbers,
                (countryId, item) => new Competitor(countryId, item.runningOrderSpot, item.finishingPosition)
            )
            .ToList();

        List<Jury> juries = countryIds.Select(countryId => new Jury(countryId)).ToList();

        List<Televote> televotes = countryIds.Select(countryId => new Televote(countryId)).ToList();

        return new Broadcast(id, date, contestStage, parentContestId, competitors, juries, televotes);
    }
}
