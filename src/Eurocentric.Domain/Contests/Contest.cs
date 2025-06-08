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
    ///     Begins the process of creating a new <see cref="Contest" /> instance using <see cref="ContestFormat.Stockholm" />
    ///     contest format using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="ContestBuilder" />A new <see cref="ContestBuilder" /> instance.</returns>
    public static ContestBuilder CreateStockholmFormat() => new StockholmFormatContest.Builder();

    /// <summary>
    ///     Begins the process of creating a new <see cref="Contest" /> instance using <see cref="ContestFormat.Liverpool" />
    ///     contest format using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="ContestBuilder" />A new <see cref="ContestBuilder" /> instance.</returns>
    public static ContestBuilder CreateLiverpoolFormat() => new LiverpoolFormatContest.Builder();
}
