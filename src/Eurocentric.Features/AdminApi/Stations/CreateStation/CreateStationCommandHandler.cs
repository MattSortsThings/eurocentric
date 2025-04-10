using ErrorOr;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.AdminApi.Stations.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.Stations.CreateStation;

internal sealed class CreateStationCommandHandler : RequestHandler<CreateStationCommand, CreateStationResponse>
{
    public override Task<ErrorOr<CreateStationResponse>> OnHandle(CreateStationCommand request,
        CancellationToken cancellationToken = default)
    {
        Station? station = request.Line switch
        {
            Line.Jubilee => new Station(1, request.Name, request.Line),
            Line.Metropolitan => new Station(2, request.Name, request.Line),
            Line.Northern => new Station(3, request.Name, request.Line),
            _ => null
        };

        ErrorOr<CreateStationResponse> result = station is not null ? new CreateStationResponse(station) : Error.Validation();

        return Task.FromResult(result);
    }
}
