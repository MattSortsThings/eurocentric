using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Events;
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

    /// <summary>
    ///     Awards a set of jury points from a single jury to all the competitors in the broadcast.
    /// </summary>
    /// <param name="votingCountryId">
    ///     The <see cref="Jury.VotingCountryId" /> value of the <see cref="Jury" /> to award its points.
    /// </param>
    /// <param name="rankedCompetingCountryIds">
    ///     The <see cref="Competitor.CompetingCountryId" /> values of all the competitors
    ///     in the broadcast, excluding the <paramref name="votingCountryId" /> argument, ordered from first place to last
    ///     place as decided by the televote.
    /// </param>
    /// <returns>
    ///     An <see cref="Updated" /> value if the broadcast was successfully updated; otherwise, a list of
    ///     <see cref="Error" /> values.
    /// </returns>
    public ErrorOr<Updated> AwardJuryPoints(CountryId votingCountryId, IEnumerable<CountryId> rankedCompetingCountryIds)
    {
        ArgumentNullException.ThrowIfNull(votingCountryId);
        ArgumentNullException.ThrowIfNull(rankedCompetingCountryIds);

        ErrorOr<Jury> errorsOrJury = TryGetJury(votingCountryId);
        ErrorOr<List<Competitor>> errorsOrRankedCompetitors =
            TryGetRankedCompetitors(votingCountryId, rankedCompetingCountryIds);

        return Tuple.Create(errorsOrJury, errorsOrRankedCompetitors)
            .Combine()
            .ThenDo(tuple => tuple.Item1.AwardPoints(tuple.Item2))
            .ThenDo(_ => UpdateCompetitorFinishingPositions())
            .ThenDo(_ => UpdateBroadcastStatus())
            .Then(_ => Result.Updated);
    }

    /// <summary>
    ///     Awards a set of televote points from a single televote to all the competitors in the broadcast.
    /// </summary>
    /// <param name="votingCountryId">
    ///     The <see cref="Televote.VotingCountryId" /> value of the <see cref="Televote" /> to award its points.
    /// </param>
    /// <param name="rankedCompetingCountryIds">
    ///     The <see cref="Competitor.CompetingCountryId" /> values of all the competitors
    ///     in the broadcast, excluding the <paramref name="votingCountryId" /> argument, ordered from first place to last
    ///     place as decided by the televote.
    /// </param>
    /// <returns>
    ///     An <see cref="Updated" /> value if the broadcast was successfully updated; otherwise, a list of
    ///     <see cref="Error" /> values.
    /// </returns>
    public ErrorOr<Updated> AwardTelevotePoints(CountryId votingCountryId, IEnumerable<CountryId> rankedCompetingCountryIds)
    {
        ArgumentNullException.ThrowIfNull(votingCountryId);
        ArgumentNullException.ThrowIfNull(rankedCompetingCountryIds);

        ErrorOr<Televote> errorsOrTelevote = TryGetTelevote(votingCountryId);
        ErrorOr<List<Competitor>> errorsOrRankedCompetitors =
            TryGetRankedCompetitors(votingCountryId, rankedCompetingCountryIds);

        return Tuple.Create(errorsOrTelevote, errorsOrRankedCompetitors)
            .Combine()
            .ThenDo(tuple => tuple.Item1.AwardPoints(tuple.Item2))
            .ThenDo(_ => UpdateCompetitorFinishingPositions())
            .ThenDo(_ => UpdateBroadcastStatus())
            .Then(_ => Result.Updated);
    }

    /// <summary>
    ///     Removes a competitor from the broadcast.
    /// </summary>
    /// <param name="competingCountryId">
    ///     The <see cref="Competitor.CompetingCountryId" /> value of the
    ///     <see cref="Competitor" /> to be removed.
    /// </param>
    /// <returns>
    ///     An <see cref="Updated" /> value if the broadcast was successfully updated; otherwise, a list of
    ///     <see cref="Error" /> values.
    /// </returns>
    public ErrorOr<Updated> DisqualifyCompetitor(CountryId competingCountryId)
    {
        ArgumentNullException.ThrowIfNull(competingCountryId);

        return competingCountryId.ToErrorOr()
            .FailIf(_ => BroadcastStatus != BroadcastStatus.Initialized, BroadcastErrors.CannotDisqualify())
            .Then(TryRemoveCompetitor);
    }

    private ErrorOr<Updated> TryRemoveCompetitor(CountryId competingCountryId)
    {
        Competitor? competitor = _competitors.FirstOrDefault(competitor => competitor.CompetingCountryId == competingCountryId);

        if (competitor is null)
        {
            return BroadcastErrors.CompetitorNotFound(Id, competingCountryId);
        }

        _competitors.Remove(competitor);
        UpdateCompetitorFinishingPositions();

        return Result.Updated;
    }

    private void UpdateCompetitorFinishingPositions()
    {
        _competitors.Sort(Competitor.GetBroadcastCompetitorComparer());

        for (int i = 0; i < _competitors.Count; i++)
        {
            _competitors[i].FinishingPosition = i + 1;
        }
    }

    private void UpdateBroadcastStatus()
    {
        BroadcastStatus priorStatus = BroadcastStatus;
        int juriesWithPointsAwarded = _juries.Count(jury => jury.PointsAwarded);
        int televotesWithPointsAwarded = _televotes.Count(televote => televote.PointsAwarded);

        BroadcastStatus = juriesWithPointsAwarded == _juries.Count && televotesWithPointsAwarded == _televotes.Count
            ? BroadcastStatus.Completed
            : juriesWithPointsAwarded == 0 && televotesWithPointsAwarded == 0
                ? BroadcastStatus.Initialized
                : BroadcastStatus.InProgress;

        if (priorStatus != BroadcastStatus)
        {
            AddDomainEvent(new BroadcastStatusUpdatedEvent(this));
        }
    }

    private ErrorOr<Jury> TryGetJury(CountryId votingCountryId)
    {
        Jury? jury = _juries.FirstOrDefault(jury => jury.VotingCountryId == votingCountryId);

        if (jury is null)
        {
            return BroadcastErrors.JuryNotFound(Id, votingCountryId);
        }

        if (jury.PointsAwarded)
        {
            return BroadcastErrors.JuryPointsAlreadyAwarded(Id, votingCountryId);
        }

        return jury;
    }

    private ErrorOr<Televote> TryGetTelevote(CountryId votingCountryId)
    {
        Televote? televote = _televotes.FirstOrDefault(televote => televote.VotingCountryId == votingCountryId);

        if (televote is null)
        {
            return BroadcastErrors.TelevoteNotFound(Id, votingCountryId);
        }

        if (televote.PointsAwarded)
        {
            return BroadcastErrors.TelevotePointsAlreadyAwarded(Id, votingCountryId);
        }

        return televote;
    }

    private ErrorOr<List<Competitor>> TryGetRankedCompetitors(CountryId votingCountryId,
        IEnumerable<CountryId> rankedCompetingCountryIds)
    {
        Dictionary<CountryId, Competitor> remainingEligibleCompetitorLookup = _competitors
            .Where(competitor => competitor.CompetingCountryId != votingCountryId)
            .ToDictionary(competitor => competitor.CompetingCountryId, competitor => competitor);

        List<ErrorOr<Competitor>> rankedCompetitors = new(remainingEligibleCompetitorLookup.Count);

        foreach (CountryId id in rankedCompetingCountryIds)
        {
            if (remainingEligibleCompetitorLookup.TryGetValue(id, out Competitor? competitor))
            {
                rankedCompetitors.Add(competitor);
                remainingEligibleCompetitorLookup.Remove(id);
            }
            else
            {
                rankedCompetitors.Add(BroadcastErrors.RankedCompetitorsMismatch());
            }
        }

        return remainingEligibleCompetitorLookup.Count != 0
            ? BroadcastErrors.RankedCompetitorsMismatch()
            : rankedCompetitors.Collect();
    }
}
