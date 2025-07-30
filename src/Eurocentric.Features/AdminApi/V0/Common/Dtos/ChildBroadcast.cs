using Eurocentric.Features.AdminApi.V0.Common.Enums;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record ChildBroadcast
{
    public required Guid BroadcastId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required bool Completed { get; init; }
}
