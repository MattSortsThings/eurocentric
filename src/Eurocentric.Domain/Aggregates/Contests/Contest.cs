using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate.
/// </summary>
public abstract class Contest : AggregateRoot<ContestId>
{
    private readonly List<ChildBroadcast> _childBroadcasts = [];
    private readonly List<Participant> _participants;

    [UsedImplicitly(Reason = "EF Core")]
    private protected Contest()
    {
        _participants = [];
    }

    private protected Contest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants) : base(id)
    {
        ContestYear = contestYear;
        CityName = cityName;
        _participants = participants;
    }

    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public ContestYear ContestYear { get; } = null!;

    /// <summary>
    ///     Gets the name of the city in which the contest is held.
    /// </summary>
    public CityName CityName { get; private init; } = null!;

    /// <summary>
    ///     Gets the contest's format.
    /// </summary>
    public abstract ContestFormat ContestFormat { get; private protected init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the contest is completed.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    ///     Gets a list of all the child broadcasts in the contest.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<ChildBroadcast> ChildBroadcasts => _childBroadcasts.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the participants in the contest.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<Participant> Participants => _participants.ToArray().AsReadOnly();

    /// <summary>
    ///     Adds a new <see cref="ChildBroadcast" /> object to the contest's <see cref="ChildBroadcasts" /> collection with the
    ///     provided <see cref="ChildBroadcast.BroadcastId" /> and <see cref="ChildBroadcast.ContestStage" /> values and a
    ///     <see cref="ChildBroadcast.Completed" /> value of <see langword="false" />.
    /// </summary>
    /// <param name="broadcastId">The ID of the broadcast aggregate.</param>
    /// <param name="contestStage">The contest stage of the broadcast aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance already contains a <see cref="ChildBroadcast" /> matching the
    ///     <paramref name="broadcastId" /> or <paramref name="contestStage" /> arguments.
    /// </exception>
    public void AddChildBroadcast(BroadcastId broadcastId, ContestStage contestStage)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        if (_childBroadcasts.Any(broadcast => broadcast.BroadcastId == broadcastId))
        {
            throw new ArgumentException("Contest already contains a ChildBroadcast object with the provided BroadcastId value.");
        }

        if (_childBroadcasts.Any(broadcast => broadcast.ContestStage == contestStage))
        {
            throw new ArgumentException(
                "Contest already contains a ChildBroadcast object with the provided ContestStage value.");
        }

        _childBroadcasts.Add(new ChildBroadcast(broadcastId, contestStage));
    }

    /// <summary>
    ///     Updates the contest to reflect the fact that the specified broadcast aggregate is now complete.
    /// </summary>
    /// <param name="broadcastId">The ID of the broadcast aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains no <see cref="ChildBroadcast" /> matching the <paramref name="broadcastId" /> argument.
    /// </exception>
    public void CompleteChildBroadcast(BroadcastId broadcastId)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        if (_childBroadcasts.FirstOrDefault(broadcast => broadcast.BroadcastId == broadcastId) is not { } childBroadcast)
        {
            throw new ArgumentException("Contest contains no ChildBroadcast object with the provided BroadcastId value.");
        }

        childBroadcast.Completed = true;
        UpdateCompleted();
    }

    /// <summary>
    ///     Updates the contest to reflect the fact that the specified broadcast aggregate has been deleted.
    /// </summary>
    /// <param name="broadcastId">The ID of the broadcast aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains no <see cref="ChildBroadcast" /> matching the <paramref name="broadcastId" /> argument.
    /// </exception>
    public void RemoveChildBroadcast(BroadcastId broadcastId)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        if (_childBroadcasts.FirstOrDefault(broadcast => broadcast.BroadcastId == broadcastId) is not { } childBroadcast)
        {
            throw new ArgumentException("Contest contains no ChildBroadcast object with the provided BroadcastId value.");
        }

        _childBroadcasts.Remove(childBroadcast);
        UpdateCompleted();
    }

    /// <summary>
    ///     Starts the process of building a new broadcast aggregate representing the First Semi-Final child broadcast of the
    ///     contest, using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="BroadcastBuilder" /> instance.</returns>
    public BroadcastBuilder CreateSemiFinal1() =>
        _childBroadcasts.Any(b => b.ContestStage == ContestStage.SemiFinal1)
            ? new ContestStageConflictBuilder(ContestStage.SemiFinal1)
            : InitializeSemiFinal1ChildBroadcastBuilder(this);

    /// <summary>
    ///     Starts the process of building a new broadcast aggregate representing the Second Semi-Final child broadcast of the
    ///     contest, using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="BroadcastBuilder" /> instance.</returns>
    public BroadcastBuilder CreateSemiFinal2() =>
        _childBroadcasts.Any(b => b.ContestStage == ContestStage.SemiFinal2)
            ? new ContestStageConflictBuilder(ContestStage.SemiFinal2)
            : InitializeSemiFinal2ChildBroadcastBuilder(this);

    /// <summary>
    ///     Starts the process of building a new broadcast aggregate representing the Grand Final child broadcast of the
    ///     contest, using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="BroadcastBuilder" /> instance.</returns>
    public BroadcastBuilder CreateGrandFinal() =>
        _childBroadcasts.Any(b => b.ContestStage == ContestStage.GrandFinal)
            ? new ContestStageConflictBuilder(ContestStage.GrandFinal)
            : InitializeGrandFinalChildBroadcastBuilder(this);

    /// <inheritdoc />
    public override IDomainEvent[] DetachAllDomainEvents() => DetachDomainEvents()
        .Concat(_childBroadcasts.SelectMany(broadcast => broadcast.DetachDomainEvents()))
        .Concat(_participants.SelectMany(participant => participant.DetachDomainEvents()))
        .ToArray();

    /// <summary>
    ///     Adds a <see cref="ContestDeletedEvent " /> to this instance.
    /// </summary>
    public void AddContestDeletedEvent() => AddDomainEvent(new ContestDeletedEvent(this));

    private void UpdateCompleted() => Completed =
        _childBroadcasts.Count(broadcast => broadcast.Completed) == Enum.GetValues<ContestStage>().Length;

    private protected abstract ChildBroadcastBuilder InitializeSemiFinal1ChildBroadcastBuilder(Contest parentContest);

    private protected abstract ChildBroadcastBuilder InitializeSemiFinal2ChildBroadcastBuilder(Contest parentContest);

    private protected abstract ChildBroadcastBuilder InitializeGrandFinalChildBroadcastBuilder(Contest parentContest);

    private sealed class ContestStageConflictBuilder(ContestStage contestStage) : BroadcastBuilder
    {
        public override BroadcastBuilder WithBroadcastDate(ErrorOr<BroadcastDate> errorsOrBroadcastDate) => this;

        public override BroadcastBuilder WithCompetingCountryIds(IEnumerable<CountryId> competingCountryIds)
        {
            ArgumentNullException.ThrowIfNull(competingCountryIds);

            return this;
        }

        public override ErrorOr<Broadcast> Build(Func<BroadcastId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return ContestErrors.ChildBroadcastContestStageConflict(contestStage);
        }
    }

    private protected abstract class ChildBroadcastBuilder : BroadcastBuilder
    {
        private readonly Contest _parentContest;

        protected ChildBroadcastBuilder(Contest parentContest)
        {
            _parentContest = parentContest;
        }

        private ErrorOr<BroadcastDate> ErrorsOrBroadcastDate { get; set; } = BroadcastErrors.BroadcastDateNotSet();

        private ErrorOr<List<Competitor>> ErrorsOrCompetitors { get; set; } = BroadcastErrors.CompetitorsNotSet();

        private protected abstract ContestStage ContestStage { get; }

        private protected abstract bool MayCompete(Participant participant);

        private protected abstract bool HasJury(Participant participant);

        private protected abstract bool HasTelevote(Participant participant);

        public override BroadcastBuilder WithBroadcastDate(ErrorOr<BroadcastDate> errorsOrBroadcastDate)
        {
            if (errorsOrBroadcastDate is { IsError: false, Value: var bd } && bd.Value.Year != _parentContest.ContestYear.Value)
            {
                ErrorsOrBroadcastDate = ContestErrors.ChildBroadcastDateOutOfRange(bd);
            }
            else
            {
                ErrorsOrBroadcastDate = errorsOrBroadcastDate;
            }

            return this;
        }

        public override BroadcastBuilder WithCompetingCountryIds(IEnumerable<CountryId> competingCountryIds)
        {
            ArgumentNullException.ThrowIfNull(competingCountryIds);

            ErrorsOrCompetitors = CreateCompetitors(competingCountryIds);

            return this;
        }

        public override ErrorOr<Broadcast> Build(Func<BroadcastId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return Tuple.Create(ErrorsOrBroadcastDate, ErrorsOrCompetitors)
                .Combine()
                .Then(tuple => new Broadcast(idProvider(),
                    tuple.Item1,
                    _parentContest.Id,
                    ContestStage,
                    tuple.Item2,
                    CreateJuries(),
                    CreateTelevotes()));
        }

        private List<Jury> CreateJuries() => _parentContest._participants.Where(HasJury)
            .Select(participant => participant.CreateJury()).ToList();

        private List<Televote> CreateTelevotes() => _parentContest._participants.Where(HasTelevote)
            .Select(participant => participant.CreateTelevote()).ToList();

        private ErrorOr<List<Competitor>> CreateCompetitors(IEnumerable<CountryId> competingCountryIds) => competingCountryIds
            .Select(id =>
                _parentContest._participants.SingleOrDefault(participant => participant.ParticipatingCountryId.Equals(id)))
            .Select(participant => participant is null || !MayCompete(participant)
                ? ContestErrors.ChildBroadcastCompetingCountryIdsMismatch()
                : participant.ToErrorOr())
            .ToList()
            .Collect()
            .FailIf(DuplicateCompetingCountryIds, BroadcastErrors.DuplicateCompetingCountryIds())
            .FailIf(IllegalCompetitorCount, BroadcastErrors.IllegalCompetitorCount())
            .Then(MapToOrderedCompetitors);

        private static List<Competitor> MapToOrderedCompetitors(IEnumerable<Participant> participants) =>
            participants.Select((participant, index) => participant.CreateCompetitor(index + 1)).ToList();

        private static bool IllegalCompetitorCount(ICollection<Participant> competitors) => competitors.Count < 2;

        private static bool DuplicateCompetingCountryIds(IEnumerable<Participant> competitors) =>
            competitors.GroupBy(x => x.ParticipatingCountryId).Any(g => g.Count() > 1);
    }
}
