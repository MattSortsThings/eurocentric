using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate.
/// </summary>
public abstract class Contest : AggregateRoot<ContestId>
{
    private const int MinLegalCompetitors = 2;
    private readonly List<BroadcastMemo> _childBroadcasts = [];
    private readonly List<Participant> _participants;

    private protected Contest()
    {
        _participants = [];
    }

    protected Contest(ContestId id, List<Participant> participants, ContestYear contestYear, CityName cityName) : base(id)
    {
        _participants = participants;
        ContestYear = contestYear;
        CityName = cityName;
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
    ///     Gets a boolean value indicating whether the contest is completed.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    ///     Gets the contest's format.
    /// </summary>
    public abstract ContestFormat ContestFormat { get; private protected init; }

    /// <summary>
    ///     Gets a list of memos of all the child broadcasts for the contest, ordered by contest stage value.
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
    ///     Adds a new <see cref="BroadcastMemo" /> to the contest's <see cref="ChildBroadcasts" /> collection and updates the
    ///     <see cref="Completed" /> value.
    /// </summary>
    /// <param name="broadcastId">The child broadcast ID.</param>
    /// <param name="contestStage">The child contest stage.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     An existing child broadcast has the same <see cref="BroadcastMemo.BroadcastId" /> value as the
    ///     <paramref name="broadcastId" /> argument; or, an existing child broadcast has the same
    ///     <see cref="BroadcastMemo.ContestStage" /> value as the <paramref name="contestStage" /> argument
    /// </exception>
    public void AddChildBroadcast(BroadcastId broadcastId, ContestStage contestStage)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        if (_childBroadcasts.Any(memo => memo.BroadcastId == broadcastId))
        {
            throw new ArgumentException("Child broadcast exists with provided broadcast ID value.", nameof(broadcastId));
        }

        if (_childBroadcasts.Any(memo => memo.ContestStage == contestStage))
        {
            throw new ArgumentException("Child broadcast exists with provided contest stage value.", nameof(contestStage));
        }

        _childBroadcasts.Add(new BroadcastMemo(broadcastId, contestStage));

        UpdateCompleted();
    }

    /// <summary>
    ///     Starts the process of building a new <see cref="Broadcast" /> representing the "Semi-Final 1" child broadcast for
    ///     the contest.
    /// </summary>
    /// <returns>A new broadcast builder configured to build a "Semi-Final 1" child broadcast for this contest.</returns>
    public BroadcastBuilder CreateSemiFinal1ChildBroadcast() => new ChildBroadcastBuilder(this, ContestStage.SemiFinal1,
        GetBroadcastEligibilityRules(ContestStage.SemiFinal1));

    /// <summary>
    ///     Starts the process of building a new <see cref="Broadcast" /> representing the "Semi-Final 2" child broadcast for
    ///     the contest.
    /// </summary>
    /// <returns>A new broadcast builder configured to build a "Semi-Final 2" child broadcast for this contest.</returns>
    public BroadcastBuilder CreateSemiFinal2ChildBroadcast() => new ChildBroadcastBuilder(this, ContestStage.SemiFinal2,
        GetBroadcastEligibilityRules(ContestStage.SemiFinal2));

    /// <summary>
    ///     Starts the process of building a new <see cref="Broadcast" /> representing the "Grand Final" child broadcast for
    ///     the contest.
    /// </summary>
    /// <returns>A new broadcast builder configured to build a "Grand Final" child broadcast for this contest.</returns>
    public BroadcastBuilder CreateGrandFinalChildBroadcast() => new ChildBroadcastBuilder(this, ContestStage.GrandFinal,
        GetBroadcastEligibilityRules(ContestStage.GrandFinal));

    private void UpdateCompleted() => Completed = _childBroadcasts.Count(memo => memo.Completed) == 3;

    private protected abstract IBroadcastEligibilityRulesSet GetBroadcastEligibilityRules(ContestStage contestStage);

    private sealed class ChildBroadcastBuilder : BroadcastBuilder
    {
        private readonly Contest _contest;
        private readonly ContestStage _contestStage;
        private readonly IBroadcastEligibilityRulesSet _eligibilityRules;

        internal ChildBroadcastBuilder(Contest contest, ContestStage contestStage,
            IBroadcastEligibilityRulesSet eligibilityRules)
        {
            _contest = contest;
            _contestStage = contestStage;
            _eligibilityRules = eligibilityRules;
        }

        /// <inheritdoc />
        public override ErrorOr<Broadcast> Build(Func<BroadcastId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return Tuple.Create(GetErrorsOrBroadcastDate(), GetErrorsOrCompetitors())
                .Combine()
                .FailIf(_ => ChildBroadcastContestStageConflict(),
                    ContestErrors.ChildBroadcastContestStageConflict(_contestStage))
                .Then(tuple => InitializeBroadcast(tuple.Item1, tuple.Item2, idProvider));
        }

        private Broadcast InitializeBroadcast(DateOnly broadcastDate, List<Competitor> competitors, Func<BroadcastId> idProvider)
        {
            BroadcastId id = idProvider();

            List<Jury> juries = CreateJuries();
            List<Televote> televotes = CreateTelevotes();

            return new Broadcast(id, broadcastDate, _contest.Id, _contestStage, competitors, juries, televotes);
        }

        private ErrorOr<DateOnly> GetErrorsOrBroadcastDate() => ErrorsOrBroadcastDate
            .FailIf(date => _contest.ContestYear.Value != date.Year,
                ContestErrors.ChildBroadcastDateOutOfRange(ErrorsOrBroadcastDate.Value));

        private ErrorOr<List<Competitor>> GetErrorsOrCompetitors() => ErrorsOrCompetingCountryIds
            .Then(countryIds => GetErrorsOrMatchingEligibleParticipants(countryIds).Collect())
            .Then(CreateCompetitors)
            .FailIf(InsufficientCompetitors, BroadcastErrors.IllegalBroadcastSize())
            .FailIf(DuplicateCompetingCountries, BroadcastErrors.DuplicateCompetingCountryIds());

        private List<ErrorOr<Participant>> GetErrorsOrMatchingEligibleParticipants(
            IEnumerable<CountryId> competingCountryIds) =>
            competingCountryIds.GroupJoin<CountryId, Participant, CountryId, ErrorOr<Participant>>(_contest._participants,
                countryId => countryId,
                participant => participant.ParticipatingCountryId, (countryId, participants) =>
                {
                    Participant? matchingParticipant = participants.DefaultIfEmpty().FirstOrDefault();

                    return matchingParticipant is null
                        ? ContestErrors.OrphanCompetingCountryId(countryId)
                        : !_eligibilityRules.MayCompete(matchingParticipant)
                            ? ContestErrors.IneligibleCompetingCountryId(countryId, _contestStage)
                            : matchingParticipant;
                }).ToList();

        private List<Jury> CreateJuries() => _contest._participants.Where(_eligibilityRules.HasJury)
            .Select(participant => participant.CreateJury())
            .ToList();

        private List<Televote> CreateTelevotes() => _contest._participants.Where(_eligibilityRules.HasTelevote)
            .Select(participant => participant.CreateTelevote())
            .ToList();

        private bool ChildBroadcastContestStageConflict() =>
            _contest._childBroadcasts.Any(memo => memo.ContestStage == _contestStage);

        private static List<Competitor> CreateCompetitors(IEnumerable<Participant> participants) =>
            participants.Select((participant, index) => participant.CreateCompetitor(index + 1)).ToList();

        private static bool DuplicateCompetingCountries(IEnumerable<Competitor> competitors) => competitors
            .GroupBy(competitor => competitor.CompetingCountryId)
            .Any(grouping => grouping.Count() > 1);

        private static bool InsufficientCompetitors(IReadOnlyCollection<Competitor> competitors) =>
            competitors.Count < MinLegalCompetitors;
    }
}
