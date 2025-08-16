using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a broadcast aggregate.
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

    internal Broadcast(BroadcastId id,
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
    ///     Gets the broadcast's transmission date.
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
    ///     Gets a boolean value indicating whether all the points in the broadcast have been awarded.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    ///     Gets a list of all the competitors in the broadcast.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<Competitor> Competitors => _competitors.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the juries in the broadcast.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<Jury> Juries => _juries.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televotes in the broadcast.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<Televote> Televotes => _televotes.ToArray().AsReadOnly();

    /// <summary>
    ///     Awards a set of points awards from a jury to the competitors in the broadcast.
    /// </summary>
    /// <remarks>
    ///     This operation <i>EITHER</i> succeeds and modifies the instance's private data <i>OR</i> fails and rolls back
    ///     all changes.
    /// </remarks>
    /// <param name="votingCountryId">The voting country ID of the jury to award its points.</param>
    /// <param name="rankedCompetingCountryIds">An ordered list of the competing country IDs to be awarded points.</param>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> an
    ///     <see cref="Result.Updated" /> value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="votingCountryId" /> is <see langword="null" />, or
    ///     <paramref name="rankedCompetingCountryIds" /> is <see langword="null" />.
    /// </exception>
    public ErrorOr<Updated> AwardJuryPoints(CountryId votingCountryId, IReadOnlyList<CountryId> rankedCompetingCountryIds)
    {
        ArgumentNullException.ThrowIfNull(votingCountryId);
        ArgumentNullException.ThrowIfNull(rankedCompetingCountryIds);

        ErrorOr<Jury> errorsOrJury = GetJuryToAwardPoints(votingCountryId);
        ErrorOr<List<Competitor>> errorsOrRankedCompetitors =
            GetCompetitorsToReceivePoints(votingCountryId, rankedCompetingCountryIds);

        return Tuple.Create(errorsOrJury, errorsOrRankedCompetitors)
            .Combine()
            .ThenDo(tuple => tuple.Item1.AwardPoints(tuple.Item2))
            .ThenDo(_ => UpdateCompetitorFinishingPositions())
            .ThenDo(_ => UpdatedCompleted())
            .ThenDo(_ => RaiseDomainEventIfCompleted())
            .Then(_ => Result.Updated);
    }

    /// <summary>
    ///     Awards a set of points awards from a televote to the competitors in the broadcast.
    /// </summary>
    /// <remarks>
    ///     This operation <i>EITHER</i> succeeds and modifies the instance's private data <i>OR</i> fails and rolls back
    ///     all changes.
    /// </remarks>
    /// <param name="votingCountryId">The voting country ID of the televote to award its points.</param>
    /// <param name="rankedCompetingCountryIds">An ordered list of the competing country IDs to be awarded points.</param>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> an
    ///     <see cref="Result.Updated" /> value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="votingCountryId" /> is <see langword="null" />, or
    ///     <paramref name="rankedCompetingCountryIds" /> is <see langword="null" />.
    /// </exception>
    public ErrorOr<Updated> AwardTelevotePoints(CountryId votingCountryId, IReadOnlyList<CountryId> rankedCompetingCountryIds)
    {
        ArgumentNullException.ThrowIfNull(votingCountryId);
        ArgumentNullException.ThrowIfNull(rankedCompetingCountryIds);

        ErrorOr<Televote> errorsOrTelevote = GetTelevoteToAwardPoints(votingCountryId);
        ErrorOr<List<Competitor>> errorsOrRankedCompetitors =
            GetCompetitorsToReceivePoints(votingCountryId, rankedCompetingCountryIds);

        return Tuple.Create(errorsOrTelevote, errorsOrRankedCompetitors)
            .Combine()
            .ThenDo(tuple => tuple.Item1.AwardPoints(tuple.Item2))
            .ThenDo(_ => UpdateCompetitorFinishingPositions())
            .ThenDo(_ => UpdatedCompleted())
            .ThenDo(_ => RaiseDomainEventIfCompleted())
            .Then(_ => Result.Updated);
    }

    /// <summary>
    ///     Disqualifies a competitor from the broadcast.
    /// </summary>
    /// <remarks>
    ///     When a competitor is disqualified, it is removed from the list of competitors. The remaining competitors'
    ///     finishing positions are updated to close the gap. This method can only be invoked when no jury or televote in the
    ///     broadcast has awarded its points.
    /// </remarks>
    /// <param name="competingCountryId">The voting country ID of the competitor to disqualify.</param>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> an
    ///     <see cref="Result.Updated" /> value.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="competingCountryId" /> is <see langword="null" />.</exception>
    public ErrorOr<Updated> DisqualifyCompetitor(CountryId competingCountryId)
    {
        ArgumentNullException.ThrowIfNull(competingCountryId);

        return GetCompetitorToDisqualify(competingCountryId)
            .FailIf(_ => RunningOrderLocked(), BroadcastErrors.BroadcastRunningOrderLocked())
            .ThenDo(RemoveCompetitor)
            .ThenDo(_ => UpdateCompetitorFinishingPositions())
            .Then(_ => Result.Updated);
    }

    /// <inheritdoc />
    public override IDomainEvent[] DetachAllDomainEvents() => DetachDomainEvents()
        .Concat(_juries.SelectMany(jury => jury.DetachDomainEvents()))
        .Concat(_televotes.SelectMany(televote => televote.DetachDomainEvents()))
        .Concat(_competitors.SelectMany(competitor => competitor.DetachDomainEvents()))
        .ToArray();

    /// <summary>
    ///     Adds a <see cref="BroadcastDeletedEvent " /> to this instance.
    /// </summary>
    public void AddBroadcastDeletedEvent() => AddDomainEvent(new BroadcastDeletedEvent(this));

    private ErrorOr<Jury> GetJuryToAwardPoints(CountryId votingCountryId)
    {
        Jury? jury = _juries.FirstOrDefault(jury => jury.VotingCountryId.Equals(votingCountryId));

        return jury is null || jury.PointsAwarded
            ? BroadcastErrors.JuryVotingCountryIdMismatch()
            : jury;
    }

    private ErrorOr<Televote> GetTelevoteToAwardPoints(CountryId votingCountryId)
    {
        Televote? televote = _televotes.FirstOrDefault(televote => televote.VotingCountryId.Equals(votingCountryId));

        return televote is null || televote.PointsAwarded
            ? BroadcastErrors.TelevoteVotingCountryIdMismatch()
            : televote;
    }

    private ErrorOr<List<Competitor>> GetCompetitorsToReceivePoints(CountryId votingCountryId,
        IReadOnlyList<CountryId> rankedCompetingCountryIds)
    {
        IOrderedEnumerable<CountryId> expectedCountryIds = _competitors.Select(competitor => competitor.CompetingCountryId)
            .Where(countryId => countryId != votingCountryId)
            .OrderBy(countryId => countryId);

        IOrderedEnumerable<CountryId> actualCountryIds = rankedCompetingCountryIds.OrderBy(countryId => countryId);

        return !actualCountryIds.SequenceEqual(expectedCountryIds)
            ? BroadcastErrors.RankedCompetingCountryIdsMismatch()
            : rankedCompetingCountryIds.Join(_competitors,
                    countryId => countryId,
                    competitor => competitor.CompetingCountryId,
                    (_, competitor) => competitor)
                .ToList();
    }

    private ErrorOr<Competitor> GetCompetitorToDisqualify(CountryId competingCountryId)
    {
        Competitor? competitor = _competitors.FirstOrDefault(competitor => competitor.CompetingCountryId == competingCountryId);

        return competitor is null
            ? BroadcastErrors.DisqualifiedCompetingCountryIdMismatch(competingCountryId)
            : competitor;
    }

    private void RemoveCompetitor(Competitor competitor) => _competitors.Remove(competitor);

    private void UpdateCompetitorFinishingPositions()
    {
        _competitors.Sort(Competitor.BroadcastCompetitorComparer);

        for (int i = 0; i < _competitors.Count; i++)
        {
            _competitors[i].FinishingPosition = i + 1;
        }
    }

    private void UpdatedCompleted() =>
        Completed = _juries.All(jury => jury.PointsAwarded) && _televotes.All(televote => televote.PointsAwarded);

    private void RaiseDomainEventIfCompleted()
    {
        if (Completed)
        {
            AddDomainEvent(new BroadcastCompletedEvent(this));
        }
    }

    private bool RunningOrderLocked() =>
        Completed || _juries.Any(jury => jury.PointsAwarded) || _televotes.Any(televote => televote.PointsAwarded);
}
