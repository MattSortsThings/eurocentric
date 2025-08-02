using Eurocentric.Features.AdminApi.V0.Common.Enums;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record ChildBroadcast
{
    public Guid BroadcastId { get; init; }

    public ContestStage ContestStage { get; init; }

    public bool Completed { get; init; }
}
