using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest.
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

    private protected Contest(
        ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants,
        GlobalTelevote? globalTelevote = null
    )
        : base(id)
    {
        _participants = participants;
        ContestYear = contestYear;
        CityName = cityName;
        GlobalTelevote = globalTelevote;
    }

    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public ContestYear ContestYear { get; private init; } = null!;

    /// <summary>
    ///     Gets the name of the contest's host city.
    /// </summary>
    public CityName CityName { get; private init; } = null!;

    /// <summary>
    ///     Gets the contest's rules.
    /// </summary>
    public abstract ContestRules ContestRules { get; private protected init; }

    /// <summary>
    ///     Gets a boolean value indicating whether the contest is queryable.
    /// </summary>
    public bool Queryable { get; private set; }

    /// <summary>
    ///     Gets the contest's optional global televote.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; }

    /// <summary>
    ///     Gets a list of all the contest's child broadcasts.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the contest's child broadcast list.</remarks>
    public IReadOnlyList<ChildBroadcast> ChildBroadcasts => _childBroadcasts.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the contest's participants.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a copy of the contest's participant list.</remarks>
    public IReadOnlyList<Participant> Participants => _participants.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets the contest's participants.
    /// </summary>
    /// <remarks>This internal property accesses the contest's participants list directly.</remarks>
    internal IReadOnlyCollection<Participant> ParticipantsCollection => _participants;

    /// <summary>
    ///     Initializes a fluent builder to build a child <see cref="Broadcast" /> for this instance representing the
    ///     <see cref="ContestStage.SemiFinal1" /> broadcast of the parent contest.
    /// </summary>
    /// <returns>
    ///     An instance of a type that implements <see cref="IBroadcastBuilder" />, configured to build the Semi-Final 1
    ///     child broadcast of this instance.
    /// </returns>
    public abstract IBroadcastBuilder CreateSemiFinal1Broadcast();

    /// <summary>
    ///     Initializes a fluent builder to build a child <see cref="Broadcast" /> for this instance representing the
    ///     <see cref="ContestStage.SemiFinal2" /> broadcast of the parent contest.
    /// </summary>
    /// <returns>
    ///     An instance of a type that implements <see cref="IBroadcastBuilder" />, configured to build the Semi-Final 2
    ///     child broadcast of this instance.
    /// </returns>
    public abstract IBroadcastBuilder CreateSemiFinal2Broadcast();

    /// <summary>
    ///     Initializes a fluent builder to build a child <see cref="Broadcast" /> for this instance representing the
    ///     <see cref="ContestStage.GrandFinal" /> broadcast of the parent contest.
    /// </summary>
    /// <returns>
    ///     An instance of a type that implements <see cref="IBroadcastBuilder" />, configured to build the Grand Final
    ///     child broadcast of this instance.
    /// </returns>
    public abstract IBroadcastBuilder CreateGrandFinalBroadcast();

    /// <summary>
    ///     Adds a new child broadcast with the specified <see cref="ChildBroadcast.ChildBroadcastId" /> and
    ///     <see cref="ChildBroadcast.ContestStage" /> values and a <see cref="ChildBroadcast.Completed" /> value of
    ///     <see langword="false" />.
    /// </summary>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <param name="contestStage">The broadcast's stage in the contest.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="broadcastId" /> argument or the <paramref name="contestStage" /> argument matches an existing
    ///     item in the <see cref="ChildBroadcasts" /> collection of this instance.
    /// </exception>
    public void AddChildBroadcast(BroadcastId broadcastId, ContestStage contestStage)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);
        ThrowOnChildBroadcastIdConflict(broadcastId);
        ThrowOnChildBroadcastContestStageConflict(contestStage);

        _childBroadcasts.Add(new ChildBroadcast(broadcastId, contestStage));
    }

    /// <summary>
    ///     Updates the existing child broadcast with the specified <see cref="ChildBroadcast.ChildBroadcastId" /> by setting
    ///     its <see cref="ChildBroadcast.Completed" /> value to <see langword="true" />.
    /// </summary>
    /// <remarks>The method may also update the <see cref="Queryable" /> value of this instance.</remarks>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="broadcastId" /> argument matches no existing item in the <see cref="ChildBroadcasts" />
    ///     collection of this instance.
    /// </exception>
    public void CompleteChildBroadcast(BroadcastId broadcastId)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        ChildBroadcast childBroadcast = GetChildBroadcastOrThrowIfNotFound(broadcastId);
        childBroadcast.Completed = true;
        UpdateQueryable();
    }

    /// <summary>
    ///     Removes the existing child broadcast with the specified <see cref="ChildBroadcast.ChildBroadcastId" />.
    /// </summary>
    /// <remarks>The method updates the <see cref="Queryable" /> value of this instance to <see langword="false" />.</remarks>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="broadcastId" /> argument matches no existing item in the <see cref="ChildBroadcasts" />
    ///     collection of this instance.
    /// </exception>
    public void RemoveChildBroadcast(BroadcastId broadcastId)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        ChildBroadcast childBroadcast = GetChildBroadcastOrThrowIfNotFound(broadcastId);
        _childBroadcasts.Remove(childBroadcast);
        UpdateQueryable();
    }

    /// <inheritdoc />
    public override IDomainEvent[] DetachAllDomainEvents()
    {
        IEnumerable<IDomainEvent> eventsEnumerable = GlobalTelevote is { } globalTelevote
            ? DetachDomainEvents()
                .Concat(_participants.SelectMany(participant => participant.DetachDomainEvents()))
                .Concat(globalTelevote.DetachDomainEvents())
            : DetachDomainEvents().Concat(_participants.SelectMany(participant => participant.DetachDomainEvents()));

        return eventsEnumerable.ToArray();
    }

    /// <summary>
    ///     Marks the contest for deletion by adding a <see cref="ContestDeletedEvent" />.
    /// </summary>
    public void MarkForDeletion() => AddDomainEvent(new ContestDeletedEvent(this));

    private void ThrowOnChildBroadcastIdConflict(BroadcastId broadcastId)
    {
        if (_childBroadcasts.Any(broadcast => broadcast.ChildBroadcastId.Equals(broadcastId)))
        {
            throw new ArgumentException("Contest already has a ChildBroadcast with the provided BroadcastId.");
        }
    }

    private void ThrowOnChildBroadcastContestStageConflict(ContestStage contestStage)
    {
        if (_childBroadcasts.Any(broadcast => broadcast.ContestStage == contestStage))
        {
            throw new ArgumentException("Contest already has a ChildBroadcast with the provided ContestStage.");
        }
    }

    private ChildBroadcast GetChildBroadcastOrThrowIfNotFound(BroadcastId broadcastId)
    {
        return _childBroadcasts.SingleOrDefault(broadcast => broadcast.ChildBroadcastId.Equals(broadcastId))
            ?? throw new ArgumentException("Contest has no ChildBroadcast with the provided BroadcastId.");
    }

    private void UpdateQueryable()
    {
        Queryable =
            _childBroadcasts.Count == Enum.GetValues<ContestStage>().Length
            && _childBroadcasts.All(broadcast => broadcast.Completed);
    }

    private protected abstract class ContestBuilder : IContestBuilder
    {
        private protected Result<ContestYear, IDomainError> ErrorOrContestYear { get; private set; } =
            ContestErrors.ContestYearPropertyNotSet();

        private protected Result<CityName, IDomainError> ErrorOrCityName { get; private set; } =
            ContestErrors.CityNamePropertyNotSet();

        private protected GlobalTelevote? GlobalTelevote { get; private set; }

        private protected List<Result<Participant, IDomainError>> ErrorsOrParticipants { get; } = new(6);

        public IContestBuilder AddSemiFinal1Participant(
            CountryId countryId,
            Result<ActName, IDomainError> errorOrActName,
            Result<SongTitle, IDomainError> errorOrSongTitle
        )
        {
            ArgumentNullException.ThrowIfNull(countryId);

            ErrorsOrParticipants.Add(Participant.CreateInSemiFinal1(countryId, errorOrActName, errorOrSongTitle));

            return this;
        }

        public IContestBuilder AddSemiFinal2Participant(
            CountryId countryId,
            Result<ActName, IDomainError> errorOrActName,
            Result<SongTitle, IDomainError> errorOrSongTitle
        )
        {
            ArgumentNullException.ThrowIfNull(countryId);

            ErrorsOrParticipants.Add(Participant.CreateInSemiFinal2(countryId, errorOrActName, errorOrSongTitle));

            return this;
        }

        public IContestBuilder WithContestYear(Result<ContestYear, IDomainError> errorOrContestYear)
        {
            ErrorOrContestYear = errorOrContestYear;

            return this;
        }

        public IContestBuilder WithCityName(Result<CityName, IDomainError> errorOrCityName)
        {
            ErrorOrCityName = errorOrCityName;

            return this;
        }

        public IContestBuilder WithGlobalTelevote(CountryId countryId)
        {
            ArgumentNullException.ThrowIfNull(countryId);

            GlobalTelevote = new GlobalTelevote(countryId);

            return this;
        }

        public abstract Result<Contest, IDomainError> Build(Func<ContestId> idProvider);
    }
}
