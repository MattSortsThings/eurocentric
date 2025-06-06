using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record BroadcastMemo(Guid BroadcastId, ContestStage ContestStage, BroadcastStatus BroadcastStatus)
    : IExampleProvider<BroadcastMemo>
{
    public static BroadcastMemo CreateExample() => new(ExampleIds.Broadcasts.Basel2025GrandFinal,
        ContestStage.GrandFinal,
        BroadcastStatus.InProgress);
}
