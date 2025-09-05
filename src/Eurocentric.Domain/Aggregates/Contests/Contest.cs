using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
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

    protected Contest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants,
        GlobalTelevote? globalTelevote = null) : base(id)
    {
        ContestYear = contestYear;
        CityName = cityName;
        GlobalTelevote = globalTelevote;
        _participants = participants;
    }

    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public ContestYear ContestYear { get; private init; } = null!;

    /// <summary>
    ///     Gets the city in which the contest is held.
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
    ///     Gets all the contest's child broadcasts.
    /// </summary>
    /// <remarks>
    ///     This property creates and returns a new collection every time it is accessed. No assumptions should be made
    ///     about the ordering of items.
    /// </remarks>
    public IReadOnlyList<ChildBroadcast> ChildBroadcasts => _childBroadcasts
        .ToArray()
        .AsReadOnly();

    /// <summary>
    ///     Gets all the contest's participants.
    /// </summary>
    /// <remarks>
    ///     This property creates and returns a new collection every time it is accessed. No assumptions should be made
    ///     about the ordering of items.
    /// </remarks>
    public IReadOnlyList<Participant> Participants => _participants.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets the contest's global televote, if present.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; private init; }
}
