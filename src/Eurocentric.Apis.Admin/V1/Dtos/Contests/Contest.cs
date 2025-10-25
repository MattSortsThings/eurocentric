using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Represents a contest.
/// </summary>
public sealed record Contest : ISchemaExampleProvider<Contest>
{
    /// <summary>
    ///     The contest's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The name of the contest host city.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     The contest's rules.
    /// </summary>
    public ContestRules ContestRules { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the contest is queryable.
    /// </summary>
    public bool Queryable { get; init; }

    /// <summary>
    ///     An array of all the contest's child broadcasts.
    /// </summary>
    public ChildBroadcast[] ChildBroadcasts { get; init; } = [];

    /// <summary>
    ///     The contest's optional global televote.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; init; }

    /// <summary>
    ///     An array of all the contest's participants.
    /// </summary>
    public Participant[] Participants { get; init; } = [];

    public static Contest CreateExample() =>
        new()
        {
            Id = Guid.Parse("756edfe8-b713-463e-8279-7eeb9e3a45c1"),
            ContestYear = 2025,
            CityName = "Basel",
            ContestRules = ContestRules.Liverpool,
            Queryable = true,
            ChildBroadcasts = [ChildBroadcast.CreateExample()],
            GlobalTelevote = GlobalTelevote.CreateExample(),
            Participants = [Participant.CreateExample()],
        };
}
