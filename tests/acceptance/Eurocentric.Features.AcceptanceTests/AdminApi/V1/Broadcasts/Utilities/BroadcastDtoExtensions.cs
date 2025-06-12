using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.Utilities;

internal static class BroadcastDtoExtensions
{
    internal static BroadcastMemo ToBroadcastMemoDto(this Broadcast broadcastDto) =>
        new(broadcastDto.Id, broadcastDto.ContestStage, broadcastDto.BroadcastStatus);
}
