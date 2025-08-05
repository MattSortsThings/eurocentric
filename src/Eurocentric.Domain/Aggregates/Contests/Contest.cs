using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
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

    protected Contest(ContestId id,
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
    public ContestYear ContestYear { get; private init; } = null!;

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
}
