using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record Contest : IExampleProvider<Contest>
{
    /// <summary>
    ///     The contest's unique ID.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public required int ContestYear { get; init; }

    /// <summary>
    ///     The name of the city in which the contest is held.
    /// </summary>
    public required string CityName { get; init; }

    /// <summary>
    ///     Indicates whether all the contest's child broadcasts have been initialized and completed.
    /// </summary>
    public required bool Completed { get; init; }

    /// <summary>
    ///     The contest's format.
    /// </summary>
    public required ContestFormat ContestFormat { get; init; }

    /// <summary>
    ///     The contest's memoized child broadcasts.
    /// </summary>
    public required BroadcastMemo[] ChildBroadcasts { get; init; }

    /// <summary>
    ///     The contest's participants.
    /// </summary>
    public required Participant[] Participants { get; init; }

    public static Contest CreateExample() => new()
    {
        Id = ExampleIds.Contest,
        ContestYear = 2025,
        CityName = "Basel",
        Completed = false,
        ContestFormat = ContestFormat.Liverpool,
        ChildBroadcasts =
        [
            new BroadcastMemo
            {
                BroadcastId = ExampleIds.Broadcast,
                ContestStage = ContestStage.GrandFinal,
                BroadcastStatus = BroadcastStatus.InProgress
            }
        ],
        Participants =
        [
            new Participant { ParticipantGroup = 0, ParticipatingCountryId = ExampleIds.CountryXx },
            new Participant
            {
                ParticipantGroup = 1,
                ParticipatingCountryId = ExampleIds.CountryIt,
                ActName = "Lucio Corsi",
                SongTitle = "Volevo Essere Un Duro"
            },
            new Participant
            {
                ParticipantGroup = 2,
                ParticipatingCountryId = ExampleIds.CountryAt,
                ActName = "JJ",
                SongTitle = "Wasted Love"
            }
        ]
    };
}
