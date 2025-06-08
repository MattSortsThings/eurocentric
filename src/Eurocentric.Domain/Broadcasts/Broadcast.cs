using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a broadcast aggregate.
/// </summary>
public sealed class Broadcast : AggregateRoot<BroadcastId>
{
    private readonly List<Competitor> _competitors;
    private readonly List<Jury> _juries;
    private readonly List<Televote> _televotes;

    private Broadcast()
    {
        _competitors = [];
        _juries = [];
        _televotes = [];
    }

    public Broadcast(BroadcastId id,
        BroadcastDate broadcastDate,
        ContestId parentContestId,
        ContestStage contestStage,
        List<Competitor> competitors,
        List<Jury> juries,
        List<Televote> televotes) : base(id)
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
    ///     Gets the ID of the broadcast's parent contest aggregate.
    /// </summary>
    public ContestId ParentContestId { get; private init; } = null!;

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest aggregate.
    /// </summary>
    public ContestStage ContestStage { get; private init; }

    /// <summary>
    ///     Gets the current status of the broadcast.
    /// </summary>
    public BroadcastStatus BroadcastStatus { get; private set; }

    /// <summary>
    ///     Gets a list of all the competitors in the broadcast, ordered by finishing position value.
    /// </summary>
    public IReadOnlyList<Competitor> Competitors =>
        _competitors.OrderBy(competitor => competitor.FinishingPosition)
            .ToArray()
            .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the juries in the broadcast, ordered by points awarded value then by voting country ID value.
    /// </summary>
    public IReadOnlyList<Jury> Juries => _juries
        .OrderBy(jury => jury.PointsAwarded)
        .ThenBy(jury => jury.VotingCountryId.Value)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televotes in the broadcast, ordered by points awarded value then by voting country ID value.
    /// </summary>
    public IReadOnlyList<Televote> Televotes => _televotes
        .OrderBy(televote => televote.PointsAwarded)
        .ThenBy(televote => televote.VotingCountryId.Value)
        .ToArray()
        .AsReadOnly();
}
