using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record ChildBroadcast : IExampleProvider<ChildBroadcast>
{
    public required Guid BroadcastId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required bool Completed { get; init; }

    public static ChildBroadcast CreateExample() => new()
    {
        BroadcastId = ExampleValues.BroadcastId, ContestStage = ContestStage.GrandFinal, Completed = false
    };
}
