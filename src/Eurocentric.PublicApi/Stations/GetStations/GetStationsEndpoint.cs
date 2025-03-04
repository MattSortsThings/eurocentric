using ErrorOr;
using Eurocentric.PublicApi.Common;
using Eurocentric.PublicApi.Stations.Models;
using Eurocentric.Shared.ApiAbstractions;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.PublicApi.Stations.GetStations;

internal sealed record GetStationsEndpoint : IEndpointInfo
{
    public string Name => nameof(GetStations);

    public Delegate Handler => async ([AsParameters] GetStationsQuery query,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetStationsResult> errorsOrResult = await sender.Send(query, cancellationToken);

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public HttpMethod Method => HttpMethod.Get;

    public string Route => "stations";

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 1;

    public string Summary => "Get stations";

    public string Description => "Retrieves a list of all the stations matching the query parameters.";

    public string Tag => EndpointTags.Stations;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield break;
        }
    }

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new GetStationsQuery { Line = Line.Jubilee };

            yield return new GetStationsResult([
                new Station("Battersea Power Station", Line.Jubilee),
                new Station("Waterloo", Line.Jubilee),
                new Station("Embankment", Line.Jubilee),
                new Station("Euston", Line.Jubilee),
                new Station("Mornington Crescent", Line.Jubilee)
            ]);
        }
    }
}
