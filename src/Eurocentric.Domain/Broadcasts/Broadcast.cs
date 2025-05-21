using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a broadcast in a contest.
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
        ContestId contestId,
        ContestStage contestStage,
        List<Competitor> competitors,
        List<Jury> juries,
        List<Televote> televotes) : base(id)
    {
        ContestId = contestId;
        ContestStage = contestStage;
        _competitors = competitors;
        _juries = juries;
        _televotes = televotes;
    }

    /// <summary>
    ///     Gets the ID of the broadcast's parent contest aggregate.
    /// </summary>
    public ContestId ContestId { get; private init; } = null!;

    /// <summary>
    ///     Gets the broadcast's stage in its contest.
    /// </summary>
    public ContestStage ContestStage { get; private init; }

    /// <summary>
    ///     Gets the broadcast's current status.
    /// </summary>
    public BroadcastStatus Status { get; private set; } = BroadcastStatus.Initialized;

    /// <summary>
    ///     Gets a list of all the broadcast's competitors, ordered by finishing position value.
    /// </summary>
    public IReadOnlyList<Competitor> Competitors => _competitors
        .OrderBy(competitor => competitor.FinishingPosition)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the broadcast's juries, ordered by points awarded value then by voting country ID value.
    /// </summary>
    public IReadOnlyList<Jury> Juries => _juries
        .OrderBy(jury => jury.PointsAwarded)
        .ThenBy(jury => jury.VotingCountryId)
        .ToArray();

    /// <summary>
    ///     Gets a list of all the broadcast's televotes, ordered by points awarded value then by voting country ID value.
    /// </summary>
    public IReadOnlyList<Televote> Televotes => _televotes
        .OrderBy(televote => televote.PointsAwarded)
        .ThenBy(televote => televote.VotingCountryId)
        .ToArray();
}
