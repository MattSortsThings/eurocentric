using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Contest : IExampleProvider<Contest>
{
    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public required ContestStatus ContestStatus { get; init; }

    public required BroadcastMemo[] ChildBroadcasts { get; init; }

    public required Participant[] Participants { get; init; }

    public static Contest CreateExample() => new()
    {
        Id = ExampleIds.Contests.Basel2025,
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        ContestStatus = ContestStatus.InProgress,
        ChildBroadcasts =
        [
            BroadcastMemo.CreateExample()
        ],
        Participants =
        [
            new Participant { ParticipatingCountryId = ExampleIds.Countries.RestOfTheWorld, ParticipantGroup = 0 },
            new Participant
            {
                ParticipatingCountryId = ExampleIds.Countries.Italy,
                ParticipantGroup = 1,
                ActName = "Lucio Corsi",
                SongTitle = "Volevo essere un duro"
            },
            new Participant
            {
                ParticipatingCountryId = ExampleIds.Countries.RestOfTheWorld,
                ParticipantGroup = 2,
                ActName = "JJ",
                SongTitle = "Wasted Love"
            }
        ]
    };
}
