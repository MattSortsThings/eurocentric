using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Contest : IExampleProvider<Contest>
{
    public Guid Id { get; init; }

    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;

    public ContestFormat ContestFormat { get; init; }

    public bool Completed { get; init; }

    public ChildBroadcast[] ChildBroadcasts { get; init; } = [];

    public Participant[] Participants { get; init; } = [];

    public static Contest CreateExample() => new()
    {
        Id = ExampleValues.ContestId,
        ContestYear = 2025,
        CityName = "Basel",
        Completed = false,
        ContestFormat = ContestFormat.Liverpool,
        ChildBroadcasts =
        [
            new ChildBroadcast
            {
                BroadcastId = ExampleValues.BroadcastId, ContestStage = ContestStage.SemiFinal1, Completed = false
            }
        ],
        Participants =
        [
            new Participant
            {
                ParticipatingCountryId = ExampleValues.CountryId,
                ParticipantGroup = 2,
                ActName = "JJ",
                SongTitle = "Wasted Love"
            }
        ]
    };
}
