using Eurocentric.Domain.Abstractions;
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
    }

    private BroadcastMemo RemoveExistingMemoOrThrowIfNotFound(BroadcastId broadcastId)
    {
        if (_broadcastMemos.SingleOrDefault(memo => memo.BroadcastId == broadcastId) is not { } existingMemo)
        {
            throw new ArgumentException("BroadcastMemos collection contains no item with the provided BroadcastId value.");
        }

        _broadcastMemos.Remove(existingMemo);

        return existingMemo;
    }
}
