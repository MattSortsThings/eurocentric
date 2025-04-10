using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Stations.CreateStation;

public sealed record CreateStationCommand : Request<CreateStationResponse>
{
    public required string Name { get; init; }

    public required Line Line { get; init; }
}
