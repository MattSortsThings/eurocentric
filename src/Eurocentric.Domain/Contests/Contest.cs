using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a contest aggregate.
/// </summary>
public abstract class Contest : AggregateRoot<ContestId>
{
    private readonly List<BroadcastMemo> _childBroadcasts = [];
    private readonly List<Participant> _participants;

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
    ///     Gets the current status of the contest aggregate.
    /// </summary>
    public ContestStatus ContestStatus { get; private set; } = ContestStatus.Initialized;

    /// <summary>
    ///     Gets a list of memos of all the child broadcasts for the contest, ordered by contest stage.
    /// </summary>
    public IReadOnlyList<BroadcastMemo> ChildBroadcasts => _childBroadcasts
        .OrderBy(memo => memo.ContestStage)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the participants in the contest, ordered by participant group value then by participating
    ///     country ID value.
    /// </summary>
    public IReadOnlyList<Participant> Participants => _participants
        .OrderBy(participant => participant.ParticipantGroup)
        .ThenBy(participant => participant.ParticipatingCountryId.Value)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Adds a new <see cref="BroadcastMemo" /> to this instance's <see cref="ChildBroadcasts" /> collection, with the
    ///     provided <see cref="BroadcastMemo.BroadcastId" /> and <see cref="BroadcastMemo.ContestStage" /> values and a
    ///     <see cref="BroadcastMemo.BroadcastStatus" /> value of <see cref="BroadcastStatus.Initialized" />.
    /// </summary>
    /// <param name="broadcastId">The ID of the broadcast aggregate.</param>
    /// <param name="contestStage">The contest stage of the broadcast aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance's <see cref="ChildBroadcasts" /> collection already contains a
    ///     <see cref="BroadcastMemo" /> with the <paramref name="broadcastId" /> argument or the
    ///     <paramref name="contestStage" /> argument.
    /// </exception>
    public void AddMemo(BroadcastId broadcastId, ContestStage contestStage)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        if (_childBroadcasts.Any(memo => memo.BroadcastId == broadcastId))
        {
            throw new ArgumentException("BroadcastMemo already exists with the provided BroadcastId value.");
        }

        if (_childBroadcasts.Any(memo => memo.ContestStage == contestStage))
        {
            throw new ArgumentException("BroadcastMemo already exists with the provided ContestStage value.");
        }

        _childBroadcasts.Add(new BroadcastMemo(broadcastId, contestStage, BroadcastStatus.Initialized));
        UpdateContestStatus();
    }

    /// <summary>
    ///     Adds a new <see cref="ContestDeletedEvent" /> to the contest's <see cref="Entity.DomainEvents" /> collection.
    /// </summary>
    public void RaiseContestDeletedEvent() => AddDomainEvent(new ContestDeletedEvent(this));

    /// <summary>
    ///     Begins the process of creating a new <see cref="Broadcast" /> instance representing the
    ///     <see cref="ContestStage.SemiFinal1" /> child broadcast of this contest.
    /// </summary>
    /// <returns>A new <see cref="BroadcastBuilder" /> instance.</returns>
    public BroadcastBuilder CreateSemiFinal1ChildBroadcast() => new ChildBroadcastBuilder(ContestStage.SemiFinal1,
        this,
        GetCompetitorEligibility(ContestStage.SemiFinal1),
        GetJuryEligibility(ContestStage.SemiFinal1),
        GetTelevoteEligibility(ContestStage.SemiFinal1));

    /// <summary>
    ///     Begins the process of creating a new <see cref="Broadcast" /> instance representing the
    ///     <see cref="ContestStage.SemiFinal2" /> child broadcast of this contest.
    /// </summary>
    /// <returns>A new <see cref="BroadcastBuilder" /> instance.</returns>
    public BroadcastBuilder CreateSemiFinal2ChildBroadcast() => new ChildBroadcastBuilder(ContestStage.SemiFinal2,
        this,
        GetCompetitorEligibility(ContestStage.SemiFinal2),
        GetJuryEligibility(ContestStage.SemiFinal2),
        GetTelevoteEligibility(ContestStage.SemiFinal2));

    /// <summary>
    ///     Begins the process of creating a new <see cref="Broadcast" /> instance representing the
    ///     <see cref="ContestStage.GrandFinal" /> child broadcast of this contest.
    /// </summary>
    /// <returns>A new <see cref="BroadcastBuilder" /> instance.</returns>
    public BroadcastBuilder CreateGrandFinalChildBroadcast() => new ChildBroadcastBuilder(ContestStage.GrandFinal,
        this,
        GetCompetitorEligibility(ContestStage.GrandFinal),
        GetJuryEligibility(ContestStage.GrandFinal),
        GetTelevoteEligibility(ContestStage.GrandFinal));

    /// <summary>
    ///     Begins the process of creating a new <see cref="Contest" /> instance using <see cref="ContestFormat.Stockholm" />
    ///     contest format using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="ContestBuilder" /> instance.</returns>
    public static ContestBuilder CreateStockholmFormat() => new StockholmFormatContest.Builder();

    /// <summary>
    ///     Begins the process of creating a new <see cref="Contest" /> instance using <see cref="ContestFormat.Liverpool" />
    ///     contest format using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="ContestBuilder" /> instance.</returns>
    public static ContestBuilder CreateLiverpoolFormat() => new LiverpoolFormatContest.Builder();

    private protected abstract Func<Participant, bool> GetJuryEligibility(ContestStage contestStage);

    private protected abstract Func<Participant, bool> GetTelevoteEligibility(ContestStage contestStage);

    private protected abstract Func<Participant, bool> GetCompetitorEligibility(ContestStage contestStage);

    private void UpdateContestStatus() => ContestStatus = _childBroadcasts.Count == 0
        ? ContestStatus.Initialized
        : _childBroadcasts.Count(memo => memo.BroadcastStatus == BroadcastStatus.Completed) == 3
            ? ContestStatus.Completed
            : ContestStatus.InProgress;

    private sealed class ChildBroadcastBuilder : BroadcastBuilder
    {
        private readonly Func<Participant, bool> _competitorEligibility;
        private readonly Contest _contest;
        private readonly Func<Participant, bool> _juryEligibility;
        private readonly Func<Participant, bool> _televoteEligibility;

        public ChildBroadcastBuilder(ContestStage contestStage,
            Contest contest,
            Func<Participant, bool> competitorEligibility,
            Func<Participant, bool> juryEligibility,
            Func<Participant, bool> televoteEligibility) : base(contestStage)
        {
            _contest = contest;
            _competitorEligibility = competitorEligibility;
            _juryEligibility = juryEligibility;
            _televoteEligibility = televoteEligibility;
        }

        private protected override ContestId ParentContestId => _contest.Id;

        private protected override ErrorOr<BroadcastDate> ConfirmInRange(ErrorOr<BroadcastDate> errorsOrBroadcastDate)
        {
            if (errorsOrBroadcastDate.IsError)
            {
                return errorsOrBroadcastDate;
            }

            BroadcastDate broadcastDate = errorsOrBroadcastDate.Value;

            return broadcastDate.Value.Year != _contest.ContestYear.Value
                ? ContestErrors.BroadcastDateOutOfRange(broadcastDate)
                : errorsOrBroadcastDate;
        }

        private protected override bool ChildBroadcastContestStageConflict() =>
            _contest._childBroadcasts.Any(memo => memo.ContestStage == ContestStage);

        private protected override ErrorOr<List<Competitor>> CreateCompetitors(IEnumerable<CountryId> competingCountryIds) =>
            GetErrorsOrMatchingEligibleParticipants(competingCountryIds)
                .Collect()
                .Then(CreateCompetitors)
                .FailIf(InsufficientCompetitors, BroadcastErrors.InsufficientCompetitors())
                .FailIf(DuplicateCompetingCountries, BroadcastErrors.DuplicateCompetingCountries());

        private IEnumerable<ErrorOr<Participant>> GetErrorsOrMatchingEligibleParticipants(
            IEnumerable<CountryId> competingCountryIds) =>
            competingCountryIds.GroupJoin<CountryId, Participant, CountryId, ErrorOr<Participant>>(_contest._participants,
                id => id,
                participant => participant.ParticipatingCountryId, (id, participants) =>
                {
                    Participant? matchingParticipant = participants.DefaultIfEmpty().FirstOrDefault();

                    return matchingParticipant is null
                        ? ContestErrors.OrphanCompetitor(id)
                        : !_competitorEligibility(matchingParticipant)
                            ? ContestErrors.IneligibleCompetingCountry(id, ContestStage)
                            : matchingParticipant;
                });

        private protected override List<Jury> CreateJuries() =>
            _contest._participants.Where(_juryEligibility)
                .Select(participant => participant.CreateJury())
                .ToList();

        private protected override List<Televote> CreateTelevotes() =>
            _contest._participants
                .Where(_televoteEligibility)
                .Select(participant => participant.CreateTelevote())
                .ToList();

        private static List<Competitor> CreateCompetitors(IEnumerable<Participant> participants) =>
            participants.Select((participant, index) => participant.CreateCompetitor(index + 1)).ToList();

        private static bool DuplicateCompetingCountries(IEnumerable<Competitor> competitors) => competitors
            .GroupBy(competitor => competitor.CompetingCountryId)
            .Any(grouping => grouping.Count() > 1);

        private static bool InsufficientCompetitors(IReadOnlyCollection<Competitor> competitors) =>
            competitors.Count < MinLegalCompetitors;
    }
}
