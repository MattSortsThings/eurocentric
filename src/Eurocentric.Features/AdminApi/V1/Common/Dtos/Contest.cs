using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Contest : IExampleProvider<Contest>
{
    public required Guid Id { get; init; }

    public required int Year { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat Format { get; init; }

    public required ContestStatus Status { get; init; }

    public required BroadcastMemo[] BroadcastMemos { get; init; }

    public required Participant[] Participants { get; init; }

    public static Contest CreateExample() => new()
    {
        Id = ExampleValues.ContestId,
        Year = 2025,
        CityName = "Basel",
        Format = ContestFormat.Liverpool,
        Status = ContestStatus.InProgress,
        BroadcastMemos = [BroadcastMemo.CreateExample()],
        Participants = [Participant.CreateExample()]
    };
}
