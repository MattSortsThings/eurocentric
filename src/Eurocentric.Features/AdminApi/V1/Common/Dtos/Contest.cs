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

    public required BroadcastMemo[] BroadcastMemos { get; init; }

    public required Participant[] Participants { get; init; }

    public static Contest CreateExample() => new()
    {
        Id = ExampleIds.Contests.Basel2025,
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        ContestStatus = ContestStatus.InProgress,
        BroadcastMemos =
        [
            BroadcastMemo.CreateExample()
        ],
        Participants =
        [
            Participant.CreateExample()
        ]
    };
}
