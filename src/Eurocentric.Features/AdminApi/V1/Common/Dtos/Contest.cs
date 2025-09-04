using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

/// <summary>
///     Represents a single contest.
/// </summary>
public sealed record Contest : IExampleProvider<Contest>
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
    ///     The name of the city in which the contest is held.
    /// </summary>
    public string CityName { get; init; } = string.Empty;

    /// <summary>
    ///     The contest's format.
    /// </summary>
    public ContestFormat ContestFormat { get; init; }

    /// <summary>
    ///     Indicates whether the contest is completed in the system.
    /// </summary>
    public bool Completed { get; init; }

    /// <summary>
    ///     The contest's child broadcast.
    /// </summary>
    public ChildBroadcast[] ChildBroadcasts { get; init; } = [];

    /// <summary>
    ///     The contest's participants.
    /// </summary>
    public Participant[] Participants { get; init; } = [];

    /// <summary>
    ///     The contest's global televote, if present.
    /// </summary>
    public GlobalTelevote? GlobalTelevote { get; init; }

    public static Contest CreateExample() => new()
    {
        Id = ExampleValues.ContestId,
        ContestYear = ExampleValues.ContestYear,
        CityName = ExampleValues.CityName,
        ContestFormat = ContestFormat.Liverpool,
        Completed = false,
        ChildBroadcasts = [ChildBroadcast.CreateExample()],
        Participants = [Participant.CreateExample()],
        GlobalTelevote = GlobalTelevote.CreateExample()
    };
}
