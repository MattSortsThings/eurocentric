using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.PublicApi.Stations.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.PublicApi.Stations.GetStations;

internal sealed class GetStationsQueryHandler : RequestHandler<GetStationsQuery, GetStationsResponse>
{
    public override Task<ErrorOr<GetStationsResponse>> OnHandle(GetStationsQuery request,
        CancellationToken cancellationToken = default)
    {
        Station[] stations = request.Line switch
        {
            Line.Jubilee =>
            [
                new Station("Swiss Cottage", Line.Jubilee),
                new Station("Westminster", Line.Jubilee),
                new Station("Canada Water", Line.Jubilee),
                new Station("North Greenwich", Line.Jubilee)
            ],

            Line.Metropolitan =>
            [
                new Station("Great Portland Street", Line.Metropolitan),
                new Station("Barbican", Line.Metropolitan),
                new Station("Aldgate", Line.Metropolitan)
            ],

            Line.Northern =>
            [
                new Station("Battersea Power Station", Line.Northern),
                new Station("Oval", Line.Northern),
                new Station("Elephant & Castle", Line.Northern),
                new Station("Angel", Line.Northern),
                new Station("Bank", Line.Northern),
                new Station("Mornington Crescent", Line.Northern)
            ],

            _ => throw new InvalidEnumArgumentException(nameof(request.Line), (int)request.Line, typeof(Line))
        };

        return Task.FromResult(ErrorOrFactory.From(new GetStationsResponse(stations)));
    }
}
