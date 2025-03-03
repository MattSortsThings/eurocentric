using System.ComponentModel;
using ErrorOr;
using Eurocentric.PublicApi.Stations.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.PublicApi.Stations.GetStations;

internal sealed class GetStationsHandler : QueryHandler<GetStationsQuery, GetStationsResult>
{
    public override async Task<ErrorOr<GetStationsResult>> Handle(GetStationsQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        IEnumerable<string> names = query.Line switch
        {
            Line.HammersmithAndCity => ["Goldhawk Road", "Royal Oak", "Paddington", "Barbican", "West Ham"],
            Line.Jubilee => ["Green Park", "Westminster", "Canada Water", "Canary Wharf", "North Greenwich"],
            Line.Northern => ["Battersea Power Station", "Waterloo", "Embankment", "Euston", "Mornington Crescent"],
            _ => throw new InvalidEnumArgumentException("line", (int)query.Line, typeof(Line))
        };

        Station[] stations = names.Select(name => new Station(name, query.Line)).ToArray();

        return new GetStationsResult(stations);
    }
}
