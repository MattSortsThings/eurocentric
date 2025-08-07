using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a broadcast aggregate.
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

    public Broadcast(BroadcastId id,
        BroadcastDate broadcastDate,
        ContestId parentContestId,
        ContestStage contestStage,
        List<Competitor> competitors,
        List<Jury> juries,
        List<Televote> televotes) : base(id)
    {
        _competitors = competitors;
        _juries = juries;
        _televotes = televotes;
        BroadcastDate = broadcastDate;
        ParentContestId = parentContestId;
        ContestStage = contestStage;
    }

    /// <summary>
    ///     Gets the broadcast's transmission date.
    /// </summary>
    public BroadcastDate BroadcastDate { get; private init; } = null!;

    /// <summary>
    ///     Gets the ID of the broadcast's parent contest aggregate.
    /// </summary>
    public ContestId ParentContestId { get; private init; } = null!;

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest aggregate.
    /// </summary>
    public ContestStage ContestStage { get; private init; }

    /// <summary>
    ///     Gets a boolean value indicating whether all the points in the broadcast have been awarded.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    ///     Gets a list of all the competitors in the broadcast.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<Competitor> Competitors => _competitors.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the juries in the broadcast.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<Jury> Juries => _juries.ToArray().AsReadOnly();

    /// <summary>
    ///     Gets a list of all the televotes in the broadcast.
    /// </summary>
    /// <remarks>Accessing this property creates and returns a new list populated from the instance's private data.</remarks>
    public IReadOnlyList<Televote> Televotes => _televotes.ToArray().AsReadOnly();

    public static Broadcast CreateDummyBroadcast() => new(
        BroadcastId.FromValue(Guid.NewGuid()),
        BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-17", "yyyy-MM-dd")).Value,
        ContestId.FromValue(Guid.NewGuid()),
        ContestStage.GrandFinal,
        Enumerable.Range(1, 10).Select(i => new Competitor(CountryId.FromValue(Guid.NewGuid()), i)).ToList(),
        Enumerable.Range(1, 10).Select(_ => new Jury(CountryId.FromValue(Guid.NewGuid()))).ToList(),
        Enumerable.Range(1, 10).Select(_ => new Televote(CountryId.FromValue(Guid.NewGuid()))).ToList()
    );
}
