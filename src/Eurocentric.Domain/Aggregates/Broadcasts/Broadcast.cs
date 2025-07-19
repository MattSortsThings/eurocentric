using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

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
        DateOnly broadcastDate,
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
    ///     Gets the broadcast's transmission date.
    /// </summary>
    public DateOnly BroadcastDate { get; private init; }

    /// <summary>
    ///     Gets the ID of the broadcast's parent contest aggregate.
    /// </summary>
    public ContestId ParentContestId { get; private init; } = null!;

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest aggregate.
    /// </summary>
    public ContestStage ContestStage { get; }

    /// <summary>
    ///     Gets a boolean value indicating whether the broadcast has been completed.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    ///     Gets a list of all the competitors in the broadcast, ordered by finishing position.
    /// </summary>
    public IReadOnlyList<Competitor> Competitors => _competitors.OrderBy(competitor => competitor.FinishingPosition)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the juries in the broadcast, ordered by boolean points awarded value then by voting country ID
    ///     value.
    /// </summary>
    public IReadOnlyList<Jury> Juries => _juries.OrderBy(voter => voter.PointsAwarded)
        .ThenBy(voter => voter.VotingCountryId)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televotes in the broadcast, ordered by boolean points awarded value then by voting country
    ///     ID value.
    /// </summary>
    public IReadOnlyList<Televote> Televotes => _televotes.OrderBy(voter => voter.PointsAwarded)
        .ThenBy(voter => voter.VotingCountryId)
        .ToArray()
        .AsReadOnly();
}
