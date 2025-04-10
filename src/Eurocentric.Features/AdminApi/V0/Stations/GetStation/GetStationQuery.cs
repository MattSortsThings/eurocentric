using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Stations.GetStation;

public sealed record GetStationQuery(int StationId) : Request<GetStationResponse>;
