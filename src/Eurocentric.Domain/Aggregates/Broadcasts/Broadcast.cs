using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Events;
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

    /// <summary>
    ///     Gets the broadcast's competitors.
    /// </summary>
    /// <remarks>This internal property accesses the broadcast's competitors list directly.</remarks>
    internal IReadOnlyCollection<Competitor> CompetitorsCollection => _competitors;

    /// <summary>
    ///     Gets the broadcast's juries.
    /// </summary>
    /// <remarks>This internal property accesses the broadcast's juries list directly.</remarks>
    internal IReadOnlyCollection<Jury> JuriesCollection => _juries;

    /// <summary>
    ///     Gets the broadcast's televotes.
    /// </summary>
    /// <remarks>This internal property accesses the broadcast's televotes list directly.</remarks>
    internal IReadOnlyCollection<Televote> TelevotesCollection => _televotes;

    /// <inheritdoc />
    public override IDomainEvent[] DetachAllDomainEvents()
    {
        IEnumerable<IDomainEvent> eventsEnumerable = DetachDomainEvents()
            .Concat(_competitors.SelectMany(competitor => competitor.DetachDomainEvents()))
            .Concat(_juries.SelectMany(jury => jury.DetachDomainEvents()))
            .Concat(_televotes.SelectMany(televote => televote.DetachDomainEvents()));

        return eventsEnumerable.ToArray();
    }

    /// <summary>
    ///     Awards the points from a jury to the competitors in the broadcast.
    /// </summary>
    /// <param name="awardParams">Determines the points to be awarded.</param>
    /// <returns><i>Either</i> a successful result <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="awardParams" /> is <see langword="null" />.</exception>
    public UnitResult<IDomainError> AwardJuryPoints(IAwardParams awardParams)
    {
        ArgumentNullException.ThrowIfNull(awardParams);

        return Result
            .Success<IAwardParams, IDomainError>(awardParams)
            .Ensure(BroadcastInvariants.NoJuryVotingCountryConflict(this))
            .Ensure(BroadcastInvariants.NoRankedCompetingCountriesConflict(this))
            .Tap(AwardPointsFromJuryToCompetitors)
            .Tap(UpdateCompetitorFinishingPositions)
            .Tap(UpdateCompleted)
            .Tap(AddEventIfCompleted)
            .Bind(_ => UnitResult.Success<IDomainError>());
    }

    /// <summary>
    ///     Awards the points from a televote to the competitors in the broadcast.
    /// </summary>
    /// <param name="awardParams">Determines the points to be awarded.</param>
    /// <returns><i>Either</i> a successful result <i>or</i> an error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="awardParams" /> is <see langword="null" />.</exception>
    public UnitResult<IDomainError> AwardTelevotePoints(IAwardParams awardParams)
    {
        ArgumentNullException.ThrowIfNull(awardParams);

        return Result
            .Success<IAwardParams, IDomainError>(awardParams)
            .Ensure(BroadcastInvariants.NoTelevoteVotingCountryConflict(this))
            .Ensure(BroadcastInvariants.NoRankedCompetingCountriesConflict(this))
            .Tap(AwardPointsFromTelevoteToCompetitors)
            .Tap(UpdateCompetitorFinishingPositions)
            .Tap(UpdateCompleted)
            .Tap(AddEventIfCompleted)
            .Bind(_ => UnitResult.Success<IDomainError>());
    }

    /// <summary>
    ///     Marks the broadcast for deletion by adding a <see cref="BroadcastDeletedEvent" />.
    /// </summary>
    public void MarkForDeletion() => AddDomainEvent(new BroadcastDeletedEvent(this));

    private void AwardPointsFromJuryToCompetitors(IAwardParams awardParams)
    {
        Jury jury = GetJuryToGivePoints(awardParams.VotingCountryId);
        Competitor[] competitors = GetRankedCompetitorsToReceivePoints(awardParams.RankedCompetingCountryIds);

        jury.AwardPoints(competitors);
    }

    private void AwardPointsFromTelevoteToCompetitors(IAwardParams awardParams)
    {
        Televote televote = GetTelevoteToGivePoints(awardParams.VotingCountryId);
        Competitor[] competitors = GetRankedCompetitorsToReceivePoints(awardParams.RankedCompetingCountryIds);

        televote.AwardPoints(competitors);
    }

    private Jury GetJuryToGivePoints(CountryId votingCountryId) =>
        _juries.Single(jury => jury.VotingCountryId.Equals(votingCountryId));

    private Televote GetTelevoteToGivePoints(CountryId votingCountryId) =>
        _televotes.Single(televote => televote.VotingCountryId.Equals(votingCountryId));

    private Competitor[] GetRankedCompetitorsToReceivePoints(IEnumerable<CountryId> rankedCompetingCountryIds)
    {
        return rankedCompetingCountryIds
            .Join(
                _competitors,
                countryId => countryId,
                competitor => competitor.CompetingCountryId,
                (_, competitor) => competitor
            )
            .ToArray();
    }

    private void UpdateCompetitorFinishingPositions()
    {
        IEnumerable<FinishingPosition> finishingPositions = FinishingPosition.CreateSequence(_competitors.Count);

        _competitors.Sort(Competitor.BroadcastCompetitorComparer);

        foreach ((Competitor competitor, FinishingPosition finishingPosition) in _competitors.Zip(finishingPositions))
        {
            competitor.FinishingPosition = finishingPosition;
        }
    }

    private void UpdateCompleted() =>
        Completed = _juries.All(jury => jury.PointsAwarded) && _televotes.All(televote => televote.PointsAwarded);

    private void AddEventIfCompleted()
    {
        if (Completed)
        {
            AddDomainEvent(new BroadcastCompletedEvent(this));
        }
    }

    internal abstract class Builder : IBroadcastBuilder
    {
        private Result<BroadcastDate, IDomainError> ErrorOrBroadcastDate { get; set; } =
            BroadcastErrors.BroadcastDatePropertyNotSet();

        private List<CountryId?> CompetingCountryIds { get; } = [];

        private protected abstract Contest ParentContest { get; }

        private protected abstract ContestStage ContestStage { get; }

        public IBroadcastBuilder WithBroadcastDate(Result<BroadcastDate, IDomainError> errorOrBroadcastDate)
        {
            ErrorOrBroadcastDate = errorOrBroadcastDate;

            return this;
        }

        public IBroadcastBuilder WithCompetingCountries(params CountryId?[] competingCountryIds)
        {
            ArgumentNullException.ThrowIfNull(competingCountryIds);

            CompetingCountryIds.Clear();
            CompetingCountryIds.EnsureCapacity(competingCountryIds.Length);
            CompetingCountryIds.AddRange(competingCountryIds);

            return this;
        }

        public Result<Broadcast, IDomainError> Build(Func<BroadcastId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return ErrorOrBroadcastDate
                .Map(InitializeWithDummyId)
                .Ensure(BroadcastInvariants.LegalCompetitorsCount)
                .Ensure(BroadcastInvariants.LegalCompetingCountries)
                .Ensure(BroadcastInvariants.HasUniqueContestStageForParentContest(ParentContest))
                .Ensure(BroadcastInvariants.BroadcastDateMatchesParentContestYear(ParentContest))
                .Ensure(BroadcastInvariants.EveryCompetitorMatchesEligibleParticipantInParentContest(ParentContest))
                .Tap(broadcast => broadcast.Id = idProvider.Invoke())
                .Tap(broadcast => broadcast.AddDomainEvent(new BroadcastCreatedEvent(broadcast)))
                .Map(broadcast => broadcast);
        }

        private List<Competitor> CreateCompetitors()
        {
            int count = CompetingCountryIds.Count;

            return CompetingCountryIds
                .Zip(
                    RunningOrderSpot.CreateSequence(count),
                    (countryId, runningOrderSpot) => (countryId, runningOrderSpot)
                )
                .Where(tuple => tuple.countryId is not null)
                .Zip(
                    FinishingPosition.CreateSequence(count),
                    (tuple, finishingPosition) =>
                        new Competitor(tuple.countryId!, tuple.runningOrderSpot, finishingPosition)
                )
                .ToList();
        }

        private protected abstract List<Jury> CreateJuries();

        private protected abstract List<Televote> CreateTelevotes();

        private Broadcast InitializeWithDummyId(BroadcastDate broadcastDate)
        {
            BroadcastId dummyId = BroadcastId.FromValue(Guid.Empty);

            return new Broadcast(
                dummyId,
                broadcastDate,
                ContestStage,
                ParentContest.Id,
                CreateCompetitors(),
                CreateJuries(),
                CreateTelevotes()
            );
        }
    }
}
