using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Broadcast : IExampleProvider<Broadcast>
{
    public required Guid Id { get; init; }

    public required Guid ContestId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required BroadcastStatus Status { get; init; }

    public required Competitor[] Competitors { get; init; }

    public required Vote[] Televotes { get; init; }

    public required Vote[] Juries { get; init; }

    public static Broadcast CreateExample() => new()
    {
        Id = ExampleValues.BroadcastId,
        ContestId = ExampleValues.ContestId,
        ContestStage = ContestStage.GrandFinal,
        Status = BroadcastStatus.InProgress,
        Competitors = [Competitor.CreateExample()],
        Televotes = [Vote.CreateExample()],
        Juries = [Vote.CreateExample()]
    };
}
