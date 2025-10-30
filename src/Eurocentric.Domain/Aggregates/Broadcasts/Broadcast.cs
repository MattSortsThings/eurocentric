using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
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
                .Ensure(BroadcastInvariants.BroadcastDateMatchesParentContestYear(ParentContest))
                .Ensure(BroadcastInvariants.EveryCompetitorMatchesEligibleParticipantInParentContest(ParentContest))
                .Tap(broadcast => broadcast.Id = idProvider.Invoke());
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
