using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a contest aggregate.
/// </summary>
public abstract class Contest : AggregateRoot<ContestId>
{
    private readonly List<BroadcastMemo> _broadcastMemos = [];
    private readonly List<Participant> _participants;

    private protected Contest()
    {
        _participants = [];
    }

    protected Contest(ContestId id, ContestYear year, CityName cityName, List<Participant> participants)
    {
        Id = id;
        Year = year;
        CityName = cityName;
        _participants = participants;
    }

    /// <summary>
    ///     Gets the contest's year.
    /// </summary>
    public ContestYear Year { get; private init; } = null!;

    /// <summary>
    ///     Gets the contest's city name.
    /// </summary>
    public CityName CityName { get; private init; } = null!;

    /// <summary>
    ///     Gets the contest's format.
    /// </summary>
    public abstract ContestFormat Format { get; private protected init; }

    /// <summary>
    ///     Gets the contest's current status.
    /// </summary>
    public ContestStatus Status { get; private set; }

    /// <summary>
    ///     Gets a list of memoized broadcast aggregates for which the contest is the parent, ordered by contest stage value.
    /// </summary>
    public IReadOnlyList<BroadcastMemo> BroadcastMemos => _broadcastMemos
        .OrderBy(memo => memo.ContestStage)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets a list of all the contest's participants, ordered by participant group value then by participating country ID
    ///     value.
    /// </summary>
    public IReadOnlyList<Participant> Participants => _participants
        .OrderBy(participant => participant.Group)
        .ThenBy(participant => participant.ParticipatingCountryId)
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Adds a new <see cref="BroadcastMemo" /> to this instance's <see cref="BroadcastMemos" /> collection, with the
    ///     provided <see cref="BroadcastMemo.BroadcastId" /> value and a <see cref="BroadcastMemo.Status" /> value of
    ///     <see cref="BroadcastStatus.Initialized" />.
    /// </summary>
    /// <param name="broadcastId">Identifies the broadcast aggregate.</param>
    /// <param name="contestStage">The broadcast aggregate's stage in the contest.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains a <see cref="BroadcastMemo" /> instance with a
    ///     <see cref="BroadcastMemo.BroadcastId" /> value equal to the <paramref name="broadcastId" /> argument; <i>OR</i>,
    ///     this instance contains a <see cref="BroadcastMemo" /> instance with a <see cref="BroadcastMemo.ContestStage" />
    ///     value equal to the <paramref name="contestStage" /> argument.
    /// </exception>
    public void AddMemo(BroadcastId broadcastId, ContestStage contestStage)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        if (_broadcastMemos.Any(memo => memo.BroadcastId == broadcastId))
        {
            throw new ArgumentException("BroadcastMemos collection contains an item with the provided BroadcastId value.");
        }

        if (_broadcastMemos.Any(memo => memo.ContestStage == contestStage))
        {
            throw new ArgumentException("BroadcastMemos collection contains an item with the provided ContestStage value.");
        }

        _broadcastMemos.Add(new BroadcastMemo(broadcastId, contestStage, BroadcastStatus.Initialized));
        UpdateStatus();
    }

    /// <summary>
    ///     Replaces an existing <see cref="BroadcastMemo" /> in this instance's <see cref="BroadcastMemos" /> collection.
    /// </summary>
    /// <param name="broadcastId">Identifies the broadcast aggregate.</param>
    /// <param name="status">The current status of the broadcast aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains no <see cref="BroadcastMemo" /> instance with a
    ///     <see cref="BroadcastMemo.BroadcastId" /> value equal to the <paramref name="broadcastId" /> argument.
    /// </exception>
    public void ReplaceMemo(BroadcastId broadcastId, BroadcastStatus status)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        BroadcastMemo removed = RemoveExistingMemoOrThrowIfNotFound(broadcastId);
        _broadcastMemos.Add(new BroadcastMemo(removed.BroadcastId, removed.ContestStage, status));
        UpdateStatus();
    }

    /// <summary>
    ///     Removes an existing <see cref="BroadcastMemo" /> from this instance's <see cref="BroadcastMemos" /> collection.
    /// </summary>
    /// <param name="broadcastId">Identifies the broadcast aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="broadcastId" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     This instance contains no <see cref="BroadcastMemo" /> instance with a
    ///     <see cref="BroadcastMemo.BroadcastId" /> value equal to the <paramref name="broadcastId" /> argument.
    /// </exception>
    public void RemoveMemo(BroadcastId broadcastId)
    {
        ArgumentNullException.ThrowIfNull(broadcastId);

        _ = RemoveExistingMemoOrThrowIfNotFound(broadcastId);
        UpdateStatus();
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Broadcast" /> aggregate in its initial state representing the
    ///     <see cref="ContestStage.SemiFinal1" /> stage of the contest.
    /// </summary>
    /// <param name="competingCountryIds">The IDs of the competing countries in their running order in the broadcast.</param>
    /// <param name="idProvider">Provides the <see cref="Broadcast.Id" /> value for the returned instance.</param>
    /// <returns>
    ///     A new <see cref="Broadcast" /> instance if it has been successfully built; otherwise, a list of
    ///     <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="competingCountryIds" /> is <see langword="null" />; or,
    ///     <paramref name="idProvider" /> is <see langword="null" />.
    /// </exception>
    public ErrorOr<Broadcast> CreateSemiFinal1Broadcast(IEnumerable<CountryId> competingCountryIds, Func<BroadcastId> idProvider)
    {
        ArgumentNullException.ThrowIfNull(competingCountryIds);
        ArgumentNullException.ThrowIfNull(idProvider);

        return InitializeSemiFinal1Builder()
            .ThenDo(builder => builder.SetCompetitors(competingCountryIds, _participants)
                .SetJuries(_participants)
                .SetTelevotes(_participants))
            .Then(builder => builder.Build(idProvider));
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Broadcast" /> aggregate in its initial state representing the
    ///     <see cref="ContestStage.SemiFinal2" /> stage of the contest.
    /// </summary>
    /// <param name="competingCountryIds">The IDs of the competing countries in their running order in the broadcast.</param>
    /// <param name="idProvider">Provides the <see cref="Broadcast.Id" /> value for the returned instance.</param>
    /// <returns>
    ///     A new <see cref="Broadcast" /> instance if it has been successfully built; otherwise, a list of
    ///     <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="competingCountryIds" /> is <see langword="null" />; or,
    ///     <paramref name="idProvider" /> is <see langword="null" />.
    /// </exception>
    public ErrorOr<Broadcast> CreateSemiFinal2Broadcast(IEnumerable<CountryId> competingCountryIds, Func<BroadcastId> idProvider)
    {
        ArgumentNullException.ThrowIfNull(competingCountryIds);
        ArgumentNullException.ThrowIfNull(idProvider);

        return InitializeSemiFinal2Builder()
            .ThenDo(builder => builder.SetCompetitors(competingCountryIds, _participants)
                .SetJuries(_participants)
                .SetTelevotes(_participants))
            .Then(builder => builder.Build(idProvider));
    }

    /// <summary>
    ///     Creates and returns a new <see cref="Broadcast" /> aggregate in its initial state representing the
    ///     <see cref="ContestStage.GrandFinal" /> stage of the contest.
    /// </summary>
    /// <param name="competingCountryIds">The IDs of the competing countries in their running order in the broadcast.</param>
    /// <param name="idProvider">Provides the <see cref="Broadcast.Id" /> value for the returned instance.</param>
    /// <returns>
    ///     A new <see cref="Broadcast" /> instance if it has been successfully built; otherwise, a list of
    ///     <see cref="Error" /> values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="competingCountryIds" /> is <see langword="null" />; or,
    ///     <paramref name="idProvider" /> is <see langword="null" />.
    /// </exception>
    public ErrorOr<Broadcast> CreateGrandFinalBroadcast(IEnumerable<CountryId> competingCountryIds, Func<BroadcastId> idProvider)
    {
        ArgumentNullException.ThrowIfNull(competingCountryIds);
        ArgumentNullException.ThrowIfNull(idProvider);

        return InitializeGrandFinalBuilder()
            .ThenDo(builder => builder.SetCompetitors(competingCountryIds, _participants)
                .SetJuries(_participants)
                .SetTelevotes(_participants))
            .Then(builder => builder.Build(idProvider));
    }

    private ErrorOr<BroadcastBuilder> InitializeSemiFinal1Builder() =>
        _broadcastMemos.Any(memo => memo.ContestStage == ContestStage.SemiFinal1)
            ? ContestErrors.BroadcastContestStageConflict(ContestStage.SemiFinal1)
            : CreateSemiFinal1BroadcastBuilder();

    private ErrorOr<BroadcastBuilder> InitializeSemiFinal2Builder() =>
        _broadcastMemos.Any(memo => memo.ContestStage == ContestStage.SemiFinal2)
            ? ContestErrors.BroadcastContestStageConflict(ContestStage.SemiFinal2)
            : CreateSemiFinal2BroadcastBuilder();

    private ErrorOr<BroadcastBuilder> InitializeGrandFinalBuilder() =>
        _broadcastMemos.Any(memo => memo.ContestStage == ContestStage.GrandFinal)
            ? ContestErrors.BroadcastContestStageConflict(ContestStage.GrandFinal)
            : CreateGrandFinalBroadcastBuilder();

    private BroadcastMemo RemoveExistingMemoOrThrowIfNotFound(BroadcastId broadcastId)
    {
        if (_broadcastMemos.SingleOrDefault(memo => memo.BroadcastId == broadcastId) is not { } existingMemo)
        {
            throw new ArgumentException("BroadcastMemos collection contains no item with the provided BroadcastId value.");
        }

        _broadcastMemos.Remove(existingMemo);

        return existingMemo;
    }

    private void UpdateStatus() => Status = _broadcastMemos.Count switch
    {
        0 => ContestStatus.Initialized,
        3 when _broadcastMemos.All(memo => memo.Status == BroadcastStatus.Completed) => ContestStatus.Completed,
        _ => ContestStatus.InProgress
    };

    private protected abstract BroadcastBuilder CreateSemiFinal1BroadcastBuilder();

    private protected abstract BroadcastBuilder CreateSemiFinal2BroadcastBuilder();

    private protected abstract BroadcastBuilder CreateGrandFinalBroadcastBuilder();
}
