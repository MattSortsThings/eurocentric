using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record BroadcastMemo(Guid BroadcastId, ContestStage ContestStage, BroadcastStatus Status)
    : IExampleProvider<BroadcastMemo>
{
    public static BroadcastMemo CreateExample() =>
        new(ExampleValues.BroadcastId, ContestStage.GrandFinal, BroadcastStatus.InProgress);
}
