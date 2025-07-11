using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

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
}
