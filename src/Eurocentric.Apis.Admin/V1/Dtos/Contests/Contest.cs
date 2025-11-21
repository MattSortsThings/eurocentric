using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Represents a contest.
/// </summary>
public sealed record Contest : IDtoSchemaExampleProvider<Contest>
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
            Id = V1ExampleIds.Contest,
            ContestYear = 2025,
            CityName = "Basel",
            ContestRules = ContestRules.Liverpool,
            GlobalTelevote = GlobalTelevote.CreateExample(),
            Participants = [Participant.CreateExample()],
        };
}
