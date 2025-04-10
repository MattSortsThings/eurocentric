using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.Stations.GetStation;

public sealed record GetStationQuery(int StationId) : Request<GetStationResponse>;
